using Asp.Versioning;
using AuthService.API.Models;
using AuthService.Domain.Entities;
using AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.SharedKernel;
using Shared.SharedKernel.CustomQuery;
using Shared.SharedKernel.Models;
using SharedKernel.Models;
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

        #region Helpers
        private string FormatErrors(IdentityResult result) =>
            string.Join("; ", result.Errors.Select(e => e.Description));

        private Task<AppUser?> FindUserByEmailAsync(string email) =>
            _userManager.FindByEmailAsync(email);

        private Task<AppUser?> FindUserByCodeAsync(string code) =>
            _userManager.Users.FirstOrDefaultAsync(u => u.Code == code);

        private Task<AppUser?> FindUserByIdAsync(string id) =>
            _userManager.FindByIdAsync(id);

        private async Task<UserDto> MapToDtoAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new UserDto
            {
                Id = user.Id,
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
        }
        #endregion

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            var users = await _userManager.Users
                .Include(u => u.UserRoles)
                .Include(u => u.Claims)
                .ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
                userDtos.Add(await MapToDtoAsync(user));

            return Ok(new ApiResponse("Fetched all users", userDtos));
        }

        [HttpGet("Search")]
        public async Task<ActionResult<ApiResponse>> SearchAccounts(
            [FromQuery] string? type,
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest(new ApiResponse("Page and PageSize must be greater than 0"));

            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(u => u.Type != null && u.Type.Trim() == type);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    (u.DisplayName ?? "").Contains(search) ||
                    (u.Code ?? "").Contains(search) ||
                    (u.Email ?? "").Contains(search));
            }

            var totalCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.DisplayName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
                userDtos.Add(await MapToDtoAsync(user));

            return Ok(new ApiResponse("Search results", new
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = userDtos
            }));
        }

        [HttpPost("Filter")]
        public async Task<ActionResult<ApiResponse>> GetByFilterPaging([FromBody] FilterRequest request)
        {
            if (request.PageIndex < 0 || request.PageSize <= 0)
                return BadRequest(new ApiResponse("PageIndex and PageSize must be greater than 0"));

            var allowedFields = new HashSet<string> { "DisplayName", "Email", "Code", "Type" };
            var users = _userManager.Users.ApplyFilterPaging(request, out int total, allowedFields).ToList();

            var dtoList = new List<UserDto>();
            foreach (var user in users)
                dtoList.Add(await MapToDtoAsync(user));

            return Ok(new ApiResponse("Filter results", new
            {
                request.PageIndex,
                request.PageSize,
                Total = total,
                Data = dtoList
            }));
        }

        [HttpGet("Detail")]
        public async Task<ActionResult<ApiResponse>> GetUserByCode(string code)
        {
            var user = await FindUserByCodeAsync(code.Trim());
            if (user == null) return NotFound(new ApiResponse("User not found"));

            return Ok(new ApiResponse("User found", await MapToDtoAsync(user)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetUserById(string id)
        {
            var user = await FindUserByIdAsync(id);
            if (user == null) return NotFound(new ApiResponse("User not found"));

            return Ok(new ApiResponse("User found", await MapToDtoAsync(user)));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse("Invalid model state", ModelState));

            if (await FindUserByEmailAsync(dto.Email) != null)
                return BadRequest(new ApiResponse($"Email {dto.Email} already exists"));

            if (!string.IsNullOrEmpty(dto.Code) && await FindUserByCodeAsync(dto.Code) != null)
                return BadRequest(new ApiResponse($"Code {dto.Code} already exists"));

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

            var password = dto.Password ?? Constants.DEFAULT_PASSWORD;
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to create user {Email}: {Errors}", dto.Email, FormatErrors(result));
                return BadRequest(new ApiResponse($"Failed to create user: {FormatErrors(result)}"));
            }

            if (dto.Roles?.Count > 0)
                foreach (var role in dto.Roles)
                    await _userManager.AddToRoleAsync(user, role);

            _logger.LogInformation("User {Email} created successfully", dto.Email);
            return Ok(new ApiResponse($"User {dto.Email} created"));
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<ApiResponse>> Update(string id, [FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse("Invalid model state", ModelState));

            var user = await FindUserByIdAsync(id);
            if (user == null)
                return NotFound(new ApiResponse("User not found"));

            if (!string.IsNullOrEmpty(dto.Code) && dto.Code != user.Code)
            {
                var existingUserCode = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Code == dto.Code && u.Id != id);

                if (existingUserCode != null)
                    return BadRequest(new ApiResponse($"Code {dto.Code} already exists with another user"));
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
                return BadRequest(new ApiResponse($"Failed to update user: {FormatErrors(result)}"));
            }

            _logger.LogInformation("User {Email} updated", dto.Email);
            return Ok(new ApiResponse($"User {dto.Email} updated"));
        }

        [HttpPut("ResetPassword/{id}")]
        public async Task<ActionResult<ApiResponse>> ResetPassword(string id, [FromBody] ResetPasswordByAdminDto dto)
        {
            var user = await FindUserByIdAsync(id);
            if (user == null)
                return NotFound(new ApiResponse("User not found"));

            var password = string.IsNullOrWhiteSpace(dto.NewPassword)
                ? Constants.DEFAULT_PASSWORD
                : dto.NewPassword.Trim();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogWarning("Failed to reset password for {Email}: {Errors}", user.Email, string.Join("; ", errors));
                return BadRequest(new ApiResponse($"Failed to reset password: {string.Join("; ", errors)}"));
            }

            _logger.LogInformation("Password reset for {Email}", user.Email);
            return Ok(new ApiResponse($"Password has been reset for {user.Email}", new { NewPassword = password }));
        }

        #region Roles
        [HttpGet("GetUserRoles")]
        public async Task<ActionResult<ApiResponse>> GetUserRoles(string email)
        {
            var user = await FindUserByEmailAsync(email);
            if (user == null) return NotFound(new ApiResponse("User not found"));

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new ApiResponse("User roles fetched", roles));
        }

        [HttpPost("AddUserToRole")]
        public async Task<ActionResult<ApiResponse>> AddUserToRole([FromBody] UserToRoleDto dto)
        {
            var user = await FindUserByEmailAsync(dto.Email);
            if (user == null) return NotFound(new ApiResponse("User not found"));

            var result = await _userManager.AddToRoleAsync(user, dto.RoleName);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse($"Failed to add role: {FormatErrors(result)}"));

            _logger.LogInformation("Added role {Role} to user {Email}", dto.RoleName, dto.Email);
            return Ok(new ApiResponse($"User {dto.Email} added to role {dto.RoleName}"));
        }

        [HttpPost("RemoveUserFromRole")]
        public async Task<ActionResult<ApiResponse>> RemoveUserFromRole([FromBody] UserToRoleDto dto)
        {
            var user = await FindUserByEmailAsync(dto.Email);
            if (user == null) return NotFound(new ApiResponse("User not found"));

            var result = await _userManager.RemoveFromRoleAsync(user, dto.RoleName);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse($"Failed to remove role: {FormatErrors(result)}"));

            _logger.LogInformation("Removed role {Role} from user {Email}", dto.RoleName, dto.Email);
            return Ok(new ApiResponse($"User {dto.Email} removed from role {dto.RoleName}"));
        }
        #endregion

        #region Claims
        [HttpGet("GetUserClaims")]
        public async Task<ActionResult<ApiResponse>> GetUserClaims(string email)
        {
            var user = await FindUserByEmailAsync(email);
            if (user == null) return NotFound(new ApiResponse("User not found"));

            var claims = await _userManager.GetClaimsAsync(user);
            return Ok(new ApiResponse("User claims fetched", claims));
        }

        [HttpPost("AddUserClaim")]
        public async Task<ActionResult<ApiResponse>> AddUserClaim([FromBody] UserToClaimDto dto)
        {
            var user = await FindUserByEmailAsync(dto.Email);
            if (user == null) return NotFound(new ApiResponse("User not found"));

            var claims = await _userManager.GetClaimsAsync(user);
            if (claims.Any(c => c.Type == Constants.CREDENTIAL_CLAIM && c.Value == dto.ClaimValue))
                return BadRequest(new ApiResponse("User already has the claim"));

            var result = await _userManager.AddClaimAsync(user, new Claim(Constants.CREDENTIAL_CLAIM, dto.ClaimValue));
            if (!result.Succeeded)
                return BadRequest(new ApiResponse($"Failed to add claim: {FormatErrors(result)}"));

            return Ok(new ApiResponse($"Claim added to {dto.Email}"));
        }

        [HttpPost("RemoveUserClaim")]
        public async Task<ActionResult<ApiResponse>> RemoveUserClaim([FromBody] UserToClaimDto dto)
        {
            var user = await FindUserByEmailAsync(dto.Email);
            if (user == null) return NotFound(new ApiResponse("User not found"));

            var result = await _userManager.RemoveClaimAsync(user, new Claim(Constants.CREDENTIAL_CLAIM, dto.ClaimValue));
            if (!result.Succeeded)
                return BadRequest(new ApiResponse($"Failed to remove claim: {FormatErrors(result)}"));

            return Ok(new ApiResponse($"Claim removed from {dto.Email}"));
        }
        #endregion
    }
}
