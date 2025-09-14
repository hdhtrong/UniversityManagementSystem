using Asp.Versioning;
using AuthService.API.Models;
using AuthService.Domain.Entities;
using AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.SharedKernel;
using System.Security.Claims;

namespace AuthService.API.Controllers
{
    [Route("api/auth/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<AppUser> userManager, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        private string FormatErrors(IdentityResult result) =>
            string.Join("; ", result.Errors.Select(e => e.Description));

        private async Task<AppUser?> FindUserByEmailAsync(string email) =>
            await _userManager.FindByEmailAsync(email);

        private async Task<AppUser?> FindUserByCodeAsync(string code) =>
           await _userManager.Users.FirstOrDefaultAsync(u => u.Code == code);

        private async Task<AppUser?> FindUserByIdAsync(string id) =>
            await _userManager.FindByIdAsync(id);

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users
                .Include(u => u.UserRoles)
                .Include(u => u.Claims)
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchAccounts(
            [FromQuery] string? type,
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest(new { error = "Page and PageSize must be greater than 0" });

            var query = _userManager.Users.AsQueryable();

            // Lọc theo Type
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(u => u.Type != null && u.Type.Trim().Equals(type));
            }

            // Search theo Fullname, Code, Email
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    (u.DisplayName != null && u.DisplayName.Contains(search)) ||
                    (u.Code != null && u.Code.Contains(search)) ||
                    (u.Email != null && u.Email.Contains(search)));
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.DisplayName) // có thể sort theo Email, Code tuỳ nhu cầu
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Email = user.Email,
                    Fullname = user.DisplayName,
                    Phone = user.PhoneNumber,
                    Code = user.Code,
                    Type = user.Type,
                    Position = user.Position,
                    DepartmentCode = user.DepartmentCode,
                    DepartmentName = user.DepartmentName,
                    Roles = roles.ToList()
                });
            }

            var result = new
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = userDtos
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await FindUserByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto
            {
                Email = user.Email,
                Fullname = user.DisplayName,
                Phone = user.PhoneNumber,
                Code = user.Code,
                Type = user.Type,
                Position = user.Position,
                DepartmentCode = user.DepartmentCode,
                DepartmentName = user.DepartmentName,
                Roles = roles.ToList()
            };

            return Ok(userDto);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // check email, username exist
            var existingUser = await FindUserByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest(new { error = $"Email {dto.Email} already exists" });
            if (!string.IsNullOrEmpty(dto.Code))
            {
                // check code exists
                var existingUserCode = await FindUserByCodeAsync(dto.Code);
                if (existingUserCode != null)
                    return BadRequest(new { error = $"Code {dto.Code} already exists" });
            }

            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = dto.Email,
                UserName = dto.Email,
                DisplayName = dto.Fullname,
                PhoneNumber = dto.Phone,
                Position = dto.Position ?? "",
                Code = dto.Code ?? "",
                Type = dto.Type ?? "",
                DepartmentCode = dto.DepartmentCode ?? "",
                DepartmentName = dto.DepartmentName ?? ""
            };

            string password = dto.Password ?? Constants.DEFAULT_PASSWORD;

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to create user {Email}: {Errors}", dto.Email, FormatErrors(result));
                return BadRequest(new { error = FormatErrors(result) });
            }

            if (dto.Roles?.Count > 0)
            {
                foreach (var role in dto.Roles)
                    await _userManager.AddToRoleAsync(user, role);
            }

            _logger.LogInformation("User {Email} created successfully", dto.Email);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new { message = $"User {dto.Email} created." });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await FindUserByIdAsync(id);
            if (user == null)
                return NotFound(new { error = "User not found" });

            // Check duplicate code (exclude current user)
            if (!string.IsNullOrEmpty(dto.Code) && !dto.Code.Equals(user.Code))
            {
                var existingUserCode = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Code == dto.Code && u.Id != id);

                if (existingUserCode != null)
                    return BadRequest(new { error = $"Code {dto.Code} already exists with another user" });
            }

            user.DisplayName = dto.Fullname;
            user.PhoneNumber = dto.Phone;
            user.Code = dto.Code;
            user.Type = dto.Type;
            user.Position = dto.Position ?? "";
            user.DepartmentCode = dto.DepartmentCode ?? "";
            user.DepartmentName = dto.DepartmentName ?? "";

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to update user {Email}: {Errors}", dto.Email, FormatErrors(result));
                return BadRequest(new { error = FormatErrors(result) });
            }

            _logger.LogInformation("User {Email} updated", dto.Email);
            return Ok(new { message = $"User {dto.Email} updated." });
        }

        [HttpPut("ResetPassword/{id}")]
        public async Task<IActionResult> ResetPassword(string id, [FromBody] ResetPasswordDto dto)
        {
            var user = await FindUserByIdAsync(id);
            if (user == null)
                return NotFound(new { error = "User not found" });

            var passwordToSet = string.IsNullOrWhiteSpace(dto.NewPassword) ? Constants.DEFAULT_PASSWORD : dto.NewPassword.Trim();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, passwordToSet);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogWarning("Failed to reset password for {Email}: {Errors}", user.Email, string.Join("; ", errors));
                return BadRequest(new { error = errors });
            }

            _logger.LogInformation("Password reset for {Email}", user.Email);
            return Ok(new { message = $"Password has been reset for {user.Email} to {passwordToSet}" });
        }

        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var user = await FindUserByEmailAsync(email);
            if (user == null)
                return NotFound(new { error = "User not found" });

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await FindUserByEmailAsync(email);
            if (user == null)
                return NotFound(new { error = "User not found" });

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                return BadRequest(new { error = FormatErrors(result) });

            _logger.LogInformation("Added role {Role} to user {Email}", roleName, email);
            return Ok(new { message = $"User {email} added to role {roleName}" });
        }

        [HttpPost("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var user = await FindUserByEmailAsync(email);
            if (user == null)
                return NotFound(new { error = "User not found" });

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
                return BadRequest(new { error = FormatErrors(result) });

            _logger.LogInformation("Removed role {Role} from user {Email}", roleName, email);
            return Ok(new { message = $"User {email} removed from role {roleName}" });
        }

        [HttpGet("GetUserClaims")]
        public async Task<IActionResult> GetUserClaims(string email)
        {
            var user = await FindUserByEmailAsync(email);
            if (user == null)
                return NotFound(new { error = "User not found" });

            var claims = await _userManager.GetClaimsAsync(user);
            return Ok(claims);
        }

        [HttpPost("AddUserClaim")]
        public async Task<IActionResult> AddUserClaim(string email, string claimValue)
        {
            var user = await FindUserByEmailAsync(email);
            if (user == null)
                return NotFound(new { error = "User not found" });

            var claims = await _userManager.GetClaimsAsync(user);
            if (claims.Any(c => c.Type == Constants.CREDENTIAL_CLAIM && c.Value == claimValue))
                return BadRequest(new { error = "User already has the claim" });

            var result = await _userManager.AddClaimAsync(user, new Claim(Constants.CREDENTIAL_CLAIM, claimValue));
            if (!result.Succeeded)
                return BadRequest(new { error = FormatErrors(result) });

            return Ok(new { message = $"Claim added to {email}" });
        }

        [HttpPost("RemoveUserClaim")]
        public async Task<IActionResult> RemoveUserClaim(string email, string claimValue)
        {
            var user = await FindUserByEmailAsync(email);
            if (user == null)
                return NotFound(new { error = "User not found" });

            var result = await _userManager.RemoveClaimAsync(user, new Claim(Constants.CREDENTIAL_CLAIM, claimValue));
            if (!result.Succeeded)
                return BadRequest(new { error = FormatErrors(result) });

            return Ok(new { message = $"Claim removed from {email}" });
        }
    }
}
