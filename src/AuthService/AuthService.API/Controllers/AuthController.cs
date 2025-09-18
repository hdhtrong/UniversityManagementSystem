using Asp.Versioning;
using AuthService.API.Models;
using AuthService.API.Utils;
using AuthService.Applications.Services;
using AuthService.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Shared.SharedAuth.Models;
using Shared.SharedKernel;
using SharedKernel.Infrastructure;
using SharedKernel.Messages;
using SharedKernel.Models;
using System.Text;

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
        public async Task<ActionResult<ApiResponse>> AuthenticatePassword([FromBody] AuthenticationPasswordDto dto)
        {
            try
            {
                _logger.LogInformation("Authentication attempt for username: {Username}", dto.Username);

                var user = await _userManager.FindByNameAsync(dto.Username);
                if (user == null)
                {
                    _logger.LogWarning("Authentication failed: Username '{Username}' not found", dto.Username);
                    return BadRequest(new ApiResponse("Username does not exist in the database!"));
                }

                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, true);
                if (!signInResult.Succeeded)
                {
                    _logger.LogWarning("Authentication failed for username: {Username} - Incorrect password", dto.Username);
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

                _logger.LogInformation("User '{Username}' authenticated successfully", dto.Username);

                return Ok(new ApiResponse("Authenticated successfully", tokens));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during authentication for username: {Username}", dto.Username);
                return StatusCode(500, new ApiResponse("Internal server error"));
            }
        }

        [HttpPut("change-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(dto.Username);
                if (user == null)
                {
                    return BadRequest(new ApiResponse("Username does not exist!"));
                }

                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, dto.CurrentPassword, true);
                if (!signInResult.Succeeded)
                {
                    return BadRequest(new ApiResponse("Incorrect current password!"));
                }

                var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(new ApiResponse("Password changed successfully"));
                }
                else
                {
                    var errs = string.Join("; ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Error changing password for {Username}: {Errors}", dto.Username, errs);
                    return BadRequest(new ApiResponse($"Issue changing password: {errs}"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred when changing password for {Username}", dto.Username);
                return StatusCode(500, new ApiResponse("Error occurred when changing password"));
            }
        }

        public record ForgotPasswordRequestDto(string Email);

        [HttpPut("forgot-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            // Tìm user theo email, không tiết lộ kết quả
            var user = await _userManager.FindByEmailAsync(dto.Email);

            // Luôn trả về generic response
            var response = new ApiResponse("If the email exists, a password reset link has been sent.");

            if (user == null)
                return Ok(response);

            // Tạo token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Encode token URL-safe bằng WebEncoders
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var encodedToken = WebEncoders.Base64UrlEncode(tokenBytes);

            // Lấy URL front-end reset password
            var frontEndUrl = _configuration["Frontend:ResetPasswordUrl"];
            frontEndUrl = string.IsNullOrWhiteSpace(frontEndUrl) ? "http://localhost:4200/reset-password" : frontEndUrl;

            // Link reset dẫn tới front-end, encode email để an toàn
            var resetLink = $"{frontEndUrl}?email={Uri.EscapeDataString(user.Email)}&token={encodedToken}";

            // Gửi email nếu RabbitMQ khả dụng
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
            }

            return Ok(response);
        }

        [HttpPut("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordByLinkDto dto)
        {
            // Decode email
            var email = Uri.UnescapeDataString(dto.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new ApiResponse("Invalid request"));

            // Xác định password: nếu user gửi thì dùng, nếu không thì tạo ngẫu nhiên
            var isGeneratedPassword = string.IsNullOrEmpty(dto.NewPassword);
            var newPassword = isGeneratedPassword ? PasswordGenerator.GeneratePassword(6) : dto.NewPassword;

            // Decode token Base64 URL-safe bằng WebEncoders
            try
            {
                var decodedBytes = WebEncoders.Base64UrlDecode(dto.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedBytes);

                var resetResult = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

                if (!resetResult.Succeeded)
                {
                    var errs = string.Join("; ", resetResult.Errors.Select(e => e.Description));
                    return BadRequest(new ApiResponse($"Failed to reset password: {errs}"));
                }
            }
            catch
            {
                return BadRequest(new ApiResponse("Invalid token format"));
            }

            // Nếu là password tự tạo, gửi email
            if (isGeneratedPassword)
            {
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
                    return Ok(new ApiResponse("Password has been reset. But email with new password could not be sent."));
                }
            }

            return Ok(new ApiResponse("Password has been reset successfully."));
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
