using Asp.Versioning;
using AuthService.Domain.Entities;
using AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.SharedKernel;
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

        protected readonly ILogger<RolesController> _logger;

        public RolesController(RoleManager<AppRole> roleManager, ILogger<RolesController> logger)
        {
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(RoleDto dto)
        {
            var roleExist = await _roleManager.RoleExistsAsync(dto.Name.Trim());
            if (!roleExist)
            {
                var role = new AppRole(dto.Name.Trim());
                role.Id = Guid.NewGuid().ToString();
                role.Description = dto.Description.Trim();
                var roleResult = await _roleManager.CreateAsync(role);

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");
                    return Ok(new { result = $"Role {dto.Name} added successfully" });
                }
                else
                {
                    string errs = "";
                    foreach (var err in roleResult.Errors)
                    {
                        errs += err.Description + "\n ";
                    }
                    _logger.LogInformation(2, "Error: " + errs);
                    return BadRequest(new { error = $"Issue adding the new {role} role, errors: " + errs });
                }
            }
            return BadRequest(new { error = $"Role {dto.Name} already exist" });
        }

        [HttpGet]
        [Route("GetRoleClaims")]
        public async Task<IActionResult> GetRoleClaims(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                return Ok(claims);
            }
            return BadRequest(new { error = "Unable to find role " + roleName });      
        }

        [HttpPost]
        [Route("AddClaimToRole")]
        public async Task<IActionResult> AddClaimToRole(string roleName, string claim)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role != null)
            {
                var result = await _roleManager.AddClaimAsync(role, new Claim(Constants.CREDENTIAL_CLAIM, claim));

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"Claim {claim} added to the {roleName} role");
                    return Ok(new { result = $"Claim {claim} added to the {roleName} role" });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: Unable to add Claim {claim} to the {roleName} role");
                    return BadRequest(new { error = $"Error: Unable to add Claim {claim} to the {roleName} role" });
                }
            }
            return BadRequest(new { error = "Unable to find role " + roleName });
        }


        [HttpPost]
        [Route("RemoveClaimFromRole")]
        public async Task<IActionResult> RemoveClaimFromRole(string roleName, string claim)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role != null)
            {
                var result = await _roleManager.RemoveClaimAsync(role, new Claim(Constants.CREDENTIAL_CLAIM, claim));

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"Claim {claim} removed from the {roleName} role");
                    return Ok(new { result = $"Claim {claim} removed from the {roleName} role" });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: Unable to remove claim {claim} from the {roleName} role");
                    return BadRequest(new { error = $"Error: Unable to remove claim {claim} from the {roleName} role" });
                }
            }
            return BadRequest(new { error = "Unable to find role " + roleName });
        }
    }
}
