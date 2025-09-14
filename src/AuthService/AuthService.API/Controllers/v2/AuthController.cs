using Asp.Versioning;
using AuthService.Applications.Services;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.SharedAuth.Models;
using Shared.SharedKernel;

namespace AuthService.API.Controllers.v2
{
    [Route("api/auth/v{version:apiVersion}/[controller]")]
    [ApiVersion("2")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtManagerService _jwtManager;

        private readonly ILogger<AuthController> _logger;

        private readonly IConfiguration _iconfiguration;

        public AuthController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IJwtManagerService jWTManager, ILogger<AuthController> logger, IConfiguration iconfiguration)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._jwtManager = jWTManager;
            this._logger = logger;
            this._iconfiguration = iconfiguration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("PasswordAuthenticate")]
        public async Task<ActionResult> PasswordAuthenticate([FromForm] string username, [FromForm] string password)
        {
            try
            {
                // check login info
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return BadRequest("Username does not exist in the database!");
                }
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, true);
                if (!signInResult.Succeeded)
                {
                    return BadRequest("Incorrect password!");
                }
                // get roles and claims              
                SignInUser signInUser = new SignInUser();
                signInUser.Email = user.Email;
                signInUser.Fullname = user.DisplayName;
                signInUser.Code = user.Code;
                FindRolesAndAllClaims(user, signInUser);
                // generate jwt token for our app
                var tokens = _jwtManager.GenerateTokens(signInUser);
                return Ok(tokens);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred when verifying username: " + username + " and password: " + password + ", err: " + ex.Message);
                return BadRequest();
            }
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromForm] string username, [FromForm] string currentPassword, [FromForm] string newPassword)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return BadRequest("Username does not exist!");
                }
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, currentPassword, true);
                if (!signInResult.Succeeded)
                {
                    return BadRequest("Incorrect current password!");
                }

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (result.Succeeded)
                {
                    return Ok("Password changed successfully");
                }
                else
                {
                    string errs = "";
                    foreach (var err in result.Errors)
                    {
                        errs += err.Description + "\n ";
                    }
                    _logger.LogInformation(2, "Error: " + errs);
                    return BadRequest(new { error = $"Issue changing password of the user {username}, errors: " + errs });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred when changing password of username: " + username + ", err: " + ex.Message);
                return BadRequest("Error occurred when changing password");
            }
        }

        private void FindRolesAndAllClaims(AppUser user, SignInUser signInUser)
        {
            // get all roles of user
            var strRoles = _userManager.GetRolesAsync(user).Result;
            List<string> allClaims = new List<string>(); // list all claims (role claims and user claims)
            // for each role, get related claims (credential) of that role
            foreach (var role in strRoles)
            {
                var appRole = _roleManager.FindByNameAsync(role).Result;
                if (appRole != null)
                {
                    var roleClaims = _roleManager.GetClaimsAsync(appRole).Result.Where(c => c.Type.Equals(Constants.CREDENTIAL_CLAIM)).Select(c => c.Value);
                    allClaims.AddRange(roleClaims);
                }
            }
            // get all claims of user
            var userClaims = _userManager.GetClaimsAsync(user).Result.Where(c => c.Type.Equals(Constants.CREDENTIAL_CLAIM)).Select(c => c.Value);
            allClaims.AddRange(userClaims);

            signInUser.Roles = strRoles.ToList();
            signInUser.Credentials = allClaims.Distinct().ToList();
        }
    }
}
