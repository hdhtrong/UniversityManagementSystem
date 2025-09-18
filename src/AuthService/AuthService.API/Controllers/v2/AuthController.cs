using Asp.Versioning;
using AuthService.API.Models;
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
using SharedKernel.Models;

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
        private readonly IPublishEndpoint _publish;
        private readonly RabbitMqHealthChecker _rabbitChecker;
        private readonly IConfiguration _configuration;

        public AuthController(
            RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IJwtManagerService jWTManager,
            ILogger<AuthController> logger,
            IPublishEndpoint publish,
            RabbitMqHealthChecker rabbitChecker,
            IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtManager = jWTManager;
            _logger = logger;
            _publish = publish;
            _rabbitChecker = rabbitChecker;
            _configuration = configuration;
        }

        [HttpPost("authenticate-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> AuthenticatePassword([FromForm] string username, [FromForm] string password)
        {
            try
            {
                _logger.LogInformation("Authentication attempt for username: {Username}", username);

                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    _logger.LogWarning("Authentication failed: Username '{Username}' not found", username);
                    return BadRequest(new ApiResponse("Username does not exist in the database!"));
                }

                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, true);
                if (!signInResult.Succeeded)
                {
                    _logger.LogWarning("Authentication failed for username: {Username} - Incorrect password", username);
                    return BadRequest(new ApiResponse("Incorrect password!"));
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

                return Ok(new ApiResponse("Authenticated successfully", tokens));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during authentication for username: {Username}", username);
                return StatusCode(500, new ApiResponse("Internal server error"));
            }
        }

        [HttpPut("change-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ChangePassword([FromForm] string username, [FromForm] string currentPassword, [FromForm] string newPassword)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return BadRequest(new ApiResponse("Username does not exist!"));
                }

                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, currentPassword, true);
                if (!signInResult.Succeeded)
                {
                    return BadRequest(new ApiResponse("Incorrect current password!"));
                }

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (result.Succeeded)
                {
                    return Ok(new ApiResponse("Password changed successfully"));
                }
                else
                {
                    var errs = string.Join("; ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Error changing password for {Username}: {Errors}", username, errs);
                    return BadRequest(new ApiResponse($"Issue changing password: {errs}"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred when changing password for {Username}", username);
                return StatusCode(500, new ApiResponse("Error occurred when changing password"));
            }
        }

        public record ForgotPasswordRequestDto(string Email);

        [HttpPut("forgot-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Ok(new ApiResponse("Ok")); // Không tiết lộ user tồn tại hay không

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);

            string resetLink;
            var baseUrl = _configuration["Frontend:ResetPasswordUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                var request = HttpContext.Request;
                var backendUrl = $"{request.Scheme}://{request.Host}";
                resetLink = $"{backendUrl}/api/v1/auth/reset-password?email={dto.Email}&token={encodedToken}";
            }
            else
            {
                resetLink = $"{baseUrl}?email={dto.Email}&token={encodedToken}";
            }

            if (await _rabbitChecker.IsAvailableAsync())
            {
                await _publish.Publish(new SendEmailMessage(
                    dto.Email,
                    "HCMIU System - Reset your password",
                    $"Click this link to reset your password: {resetLink}"
                ));
            }
            else
            {
                _logger.LogWarning("RabbitMQ not available, cannot send reset email to {Email}", dto.Email);
                return StatusCode(503, new ApiResponse("Service temporarily unavailable (email not sent)"));
            }

            return Ok(new ApiResponse("Reset link sent via email"));
        }

        [HttpPut("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordByLinkDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return BadRequest(new ApiResponse("Invalid request"));

            var newPassword = !string.IsNullOrEmpty(dto.NewPassword)
                ? dto.NewPassword.Trim()
                : PasswordGenerator.GeneratePassword(6);

            var decodedToken = System.Web.HttpUtility.UrlDecode(dto.Token);
            var resetResult = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            if (!resetResult.Succeeded)
            {
                var errs = string.Join("; ", resetResult.Errors.Select(e => e.Description));
                return BadRequest(new ApiResponse($"Failed to reset password: {errs}"));
            }

            if (await _rabbitChecker.IsAvailableAsync())
            {
                await _publish.Publish(new SendEmailMessage(
                    user.Email,
                    "HCMIU System - Your new password",
                    $"Your new password is: {newPassword}. Please change it after your first login!"
                ));
            }
            else
            {
                _logger.LogWarning("RabbitMQ not available, cannot send reset email to {Email}", dto.Email);
                return Ok(new ApiResponse($"Password has been reset to {newPassword}. But email not sent."));
            }

            return Ok(new ApiResponse($"Password has been reset to {newPassword}. Check your email."));
        }

        private void FindRolesAndAllClaims(AppUser user, SignInUser signInUser)
        {
            var strRoles = _userManager.GetRolesAsync(user).Result;
            var allClaims = new List<string>();

            foreach (var role in strRoles)
            {
                var appRole = _roleManager.FindByNameAsync(role).Result;
                if (appRole != null)
                {
                    var roleClaims = _roleManager.GetClaimsAsync(appRole).Result
                        .Where(c => c.Type.Equals(Constants.CREDENTIAL_CLAIM))
                        .Select(c => c.Value);
                    allClaims.AddRange(roleClaims);
                }
            }

            var userClaims = _userManager.GetClaimsAsync(user).Result
                .Where(c => c.Type.Equals(Constants.CREDENTIAL_CLAIM))
                .Select(c => c.Value);
            allClaims.AddRange(userClaims);

            signInUser.Roles = strRoles.ToList();
            signInUser.Credentials = allClaims.Distinct().ToList();
        }
    }
}
