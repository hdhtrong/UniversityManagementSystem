using Asp.Versioning;
using AuthService.Domain.Entities;
using AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.SharedKernel;
using System.Security.Claims;


namespace AuthService.API.Controllers.v2
{
    [Route("api/auth/v{version:apiVersion}/[controller]")]
    [ApiVersion("2")]
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
    }
}
