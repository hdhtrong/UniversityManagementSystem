using Asp.Versioning;
using AuthService.API.Models;
using AuthService.Domain.Entities;
using AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.SharedKernel;
using Shared.SharedKernel.Models;
using SharedKernel.Models;
using System.Security.Claims;

namespace AuthService.API.Controllers
{
    [Route("api/auth/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ILogger<RolesController> _logger;

        public RolesController(RoleManager<AppRole> roleManager, ILogger<RolesController> logger)
        {
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(new ApiResponse("Roles retrieved successfully", roles));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(RoleDto dto)
        {
            var roleExist = await _roleManager.RoleExistsAsync(dto.Name.Trim());
            if (roleExist)
                return BadRequest(new ApiResponse($"Role {dto.Name} already exists"));

            var role = new AppRole(dto.Name.Trim())
            {
                Id = Guid.NewGuid().ToString(),
                Description = dto.Description.Trim()
            };

            var roleResult = await _roleManager.CreateAsync(role);
            if (!roleResult.Succeeded)
            {
                var errs = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to create role {RoleName}: {Errors}", dto.Name, errs);
                return BadRequest(new ApiResponse($"Failed to create role {dto.Name}", errs));
            }

            _logger.LogInformation("Role {RoleName} created successfully", dto.Name);
            return Ok(new ApiResponse($"Role {dto.Name} created successfully", role));
        }

        [HttpGet("GetRoleClaims")]
        public async Task<IActionResult> GetRoleClaims(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return NotFound(new ApiResponse($"Role {roleName} not found"));

            var claims = await _roleManager.GetClaimsAsync(role);
            return Ok(new ApiResponse($"Claims for role {roleName} retrieved", claims));
        }

        [HttpPost("AddClaimToRole")]
        public async Task<IActionResult> AddClaimToRole([FromBody] RoleToClaimDto dto)
        {
            var role = await _roleManager.FindByNameAsync(dto.RoleName);
            if (role == null)
                return NotFound(new ApiResponse($"Role {dto.RoleName} not found"));

            var result = await _roleManager.AddClaimAsync(role, new Claim(Constants.CREDENTIAL_CLAIM, dto.ClaimValue));
            if (!result.Succeeded)
            {
                var errs = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to add claim {Claim} to role {Role}: {Errors}", dto.ClaimValue, dto.RoleName, errs);
                return BadRequest(new ApiResponse($"Failed to add claim {dto.ClaimValue} to role {dto.RoleName}", errs));
            }

            _logger.LogInformation("Claim {Claim} added to role {Role}", dto.ClaimValue, dto.RoleName);
            return Ok(new ApiResponse($"Claim {dto.ClaimValue} added to role {dto.RoleName}"));
        }

        [HttpPost("RemoveClaimFromRole")]
        public async Task<IActionResult> RemoveClaimFromRole([FromBody] RoleToClaimDto dto)
        {
            var role = await _roleManager.FindByNameAsync(dto.RoleName);
            if (role == null)
                return NotFound(new ApiResponse($"Role {dto.RoleName} not found"));

            var result = await _roleManager.RemoveClaimAsync(role, new Claim(Constants.CREDENTIAL_CLAIM, dto.ClaimValue));
            if (!result.Succeeded)
            {
                var errs = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to remove claim {Claim} from role {Role}: {Errors}", dto.ClaimValue, dto.RoleName, errs);
                return BadRequest(new ApiResponse($"Failed to remove claim {dto.ClaimValue} from role {dto.RoleName}", errs));
            }

            _logger.LogInformation("Claim {Claim} removed from role {Role}", dto.ClaimValue, dto.RoleName);
            return Ok(new ApiResponse($"Claim {dto.ClaimValue} removed from role {dto.RoleName}"));
        }
    }
}
