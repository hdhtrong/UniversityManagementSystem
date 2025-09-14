using Asp.Versioning;
using AuthService.Domain.Entities;
using AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.SharedKernel;
using System.Security.Claims;

namespace AuthService.API.Controllers.v2
{
    [Route("api/auth/v{version:apiVersion}/[controller]")]
    [ApiVersion("2")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        protected readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<AppUser> userManager, ILogger<UsersController> logger)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.Include(u => u.UserRoles).Include(u => u.Claims).ToListAsync();
            return Ok(users);
        }
    }
}
