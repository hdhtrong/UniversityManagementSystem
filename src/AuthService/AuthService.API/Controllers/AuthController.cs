using Asp.Versioning;
using AuthService.API.Utils;
using AuthService.Applications.Services;
using AuthService.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.SharedAuth.Models;
using Shared.SharedKernel;
using SharedKernel.Infrastructure;
using SharedKernel.Messages;

namespace AuthService.API.Controllers
{
    [Route("api/auth/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtManagerService _jwtManager;

        private readonly ILogger<AuthController> _logger;

        private readonly LinkGenerator _linkGenerator;

        private readonly IPublishEndpoint _publish;

        private readonly RabbitMqHealthChecker _rabbitChecker;

        public AuthController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IJwtManagerService jWTManager, ILogger<AuthController> logger, IPublishEndpoint publish, LinkGenerator linkGenerator, RabbitMqHealthChecker rabbitChecker)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtManager = jWTManager;
            _logger = logger;
            _linkGenerator = linkGenerator;
            _publish = publish;
            _rabbitChecker = rabbitChecker;
        }

        [HttpPost("authenticate-password")]
        [AllowAnonymous]
        public async Task<ActionResult> AuthenticatePassword([FromForm] string username, [FromForm] string password)
        {
            try
            {
                _logger.LogInformation("Authentication attempt for username: {Username}", username);

                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    _logger.LogWarning("Authentication failed: Username '{Username}' not found", username);
                    return BadRequest("Username does not exist in the database!");
                }

                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, true);
                if (!signInResult.Succeeded)
                {
                    _logger.LogWarning("Authentication failed for username: {Username} - Incorrect password", username);
                    return BadRequest("Incorrect password!");
                }

                var signInUser = new SignInUser
                {
                    Email = user.Email,
                    Fullname = user.DisplayName,
                    Code = user.Code
                };
                FindRolesAndAllClaims(user, signInUser);
                var tokens = _jwtManager.GenerateTokens(signInUser);

                _logger.LogInformation("User '{Username}' authenticated successfully", username);

                return Ok(tokens);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during authentication for username: {Username}", username);
                return BadRequest("Internal server error");
            }
        }

        [HttpPut("change-password")]
        [AllowAnonymous]
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

        public record ForgotPasswordRequest(string Email);

        [HttpPut("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Ok(); // Không tiết lộ user tồn tại hay không

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);

            // Sinh URL động dựa vào route + version
            var resetLink = _linkGenerator.GetUriByAction(
                HttpContext,
                action: "ResetPassword",
                controller: "Auth",
                values: new { version = "1", email = request.Email, token = encodedToken }
            );

            if (await _rabbitChecker.IsAvailableAsync())
            {
                await _publish.Publish(new SendEmailMessage(
                    request.Email,
                    "HCMIU System-Reset your password for account",
                    $"Click this link to reset: {resetLink}"
                ));
            }
            else
            {
                _logger.LogWarning("RabbitMQ is not available, cannot send reset email to {Email}", request.Email);
                return StatusCode(503, "Service temporarily unavailable (email not sent)");
            }

            return Ok("Reset link sent via email");
        }

        [HttpGet("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Invalid request");

            // Sinh password mới tạm thời
            var newPassword = PasswordGenerator.GeneratePassword(6);

            var decodedToken = System.Web.HttpUtility.UrlDecode(token);
            // Reset password
            var resetResult = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);
            if (!resetResult.Succeeded)
                return BadRequest(resetResult.Errors);

            if (await _rabbitChecker.IsAvailableAsync())
            {
                // Gửi mail thông báo password mới
                await _publish.Publish(new SendEmailMessage(
                    user.Email,
                    "HCMIU System-Your new password",
                    $"Your new password is: {newPassword}. Please change it after your first login!"));
            }
            else
            {
                _logger.LogWarning("RabbitMQ is not available, cannot send reset email to {Email}", email);
                return Ok($"Password has been reset to: {newPassword}. But email not sent.");
            }

            return Ok("Password has been reset. Check your email.");
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
