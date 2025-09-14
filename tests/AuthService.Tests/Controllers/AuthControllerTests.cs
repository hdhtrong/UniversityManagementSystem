using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AuthService.API.Controllers;
using AuthService.Domain.Entities;
using AuthService.Applications.Services;
using Shared.SharedAuth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MassTransit;
using Microsoft.AspNetCore.Routing;
using SharedKernel.Infrastructure;

namespace AuthService.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
        private readonly Mock<RoleManager<AppRole>> _roleManagerMock;
        private readonly Mock<IJwtManagerService> _jwtManagerMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IPublishEndpoint> _publishMock;
        private readonly Mock<LinkGenerator> _linkGeneratorMock;
        private readonly Mock<RabbitMqHealthChecker> _rabbitCheckerMock;

        private readonly AuthController _controller;

        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(
                store.Object, null, null, null, null, null, null, null, null);
        }

        private static Mock<SignInManager<TUser>> MockSignInManager<TUser>() where TUser : class
        {
            var userManager = MockUserManager<TUser>();
            return new Mock<SignInManager<TUser>>(
                userManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<TUser>>().Object,
                null, null, null, null);
        }

        private static Mock<RoleManager<TRole>> MockRoleManager<TRole>() where TRole : class
        {
            var store = new Mock<IRoleStore<TRole>>();
            return new Mock<RoleManager<TRole>>(
                store.Object, null, null, null, null);
        }

        public AuthControllerTests()
        {
            // UserManager mock
            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(),
                null, null, null, null, null, null, null, null
            );

            // SignInManager mock
            _signInManagerMock = new Mock<SignInManager<AppUser>>(
                _userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                null, null, null, null
            );

            // RoleManager mock
            _roleManagerMock = new Mock<RoleManager<AppRole>>(
                Mock.Of<IRoleStore<AppRole>>(),
                null, null, null, null
            );

            _jwtManagerMock = new Mock<IJwtManagerService>();
            _loggerMock = new Mock<ILogger<AuthController>>();
            _publishMock = new Mock<IPublishEndpoint>();
            _linkGeneratorMock = new Mock<LinkGenerator>();

            _controller = new AuthController(
                _roleManagerMock.Object,
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _jwtManagerMock.Object,
                _loggerMock.Object,
                _publishMock.Object,
                _linkGeneratorMock.Object,
                _rabbitCheckerMock.Object
            );
        }


        // ✅ TEST CASE 0: Username and password valid
        [Fact]
        public async Task PasswordAuthenticate_ShouldReturnTokens_WhenCredentialsAreValid()
        {
            // Arrange
            var username = "testuser";
            var password = "correctpassword";
            var user = new AppUser { UserName = username, Email = "test@example.com", DisplayName = "Test User" };

            _userManagerMock.Setup(u => u.FindByNameAsync(username))
                            .ReturnsAsync(user);

            _signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(user, password, true))
                              .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                            .ReturnsAsync(new List<string> { "Admin" });

            _roleManagerMock.Setup(r => r.FindByNameAsync("Admin"))
                            .ReturnsAsync(new AppRole { Name = "Admin" });

            _roleManagerMock.Setup(r => r.GetClaimsAsync(It.IsAny<AppRole>()))
                            .ReturnsAsync(new List<System.Security.Claims.Claim>());

            _userManagerMock.Setup(u => u.GetClaimsAsync(user))
                            .ReturnsAsync(new List<System.Security.Claims.Claim>());

            var expectedTokens = new IUTokens
            {
                Token = "access-token",
                RefreshToken = "refresh-token"
            };
            _jwtManagerMock.Setup(j => j.GenerateTokens(It.IsAny<SignInUser>()))
                           .Returns(expectedTokens);

            // Act
            var result = await _controller.AuthenticatePassword(username, password);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedTokens);
        }

        // ✅ TEST CASE 1: Username không tồn tại
        [Fact]
        public async Task PasswordAuthenticate_ShouldReturnBadRequest_WhenUserNotFound()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync("nonexistent"))
                            .ReturnsAsync((AppUser)null);

            // Act
            var result = await _controller.AuthenticatePassword("nonexistent", "anyPassword");

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().Be("Username does not exist in the database!");
        }

        // ✅ TEST CASE 2: Sai mật khẩu
        [Fact]
        public async Task PasswordAuthenticate_ShouldReturnBadRequest_WhenPasswordIncorrect()
        {
            // Arrange
            var user = new AppUser { UserName = "testuser" };

            _userManagerMock.Setup(x => x.FindByNameAsync("testuser"))
                            .ReturnsAsync(user);

            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, "wrongpassword", true))
                              .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _controller.AuthenticatePassword("testuser", "wrongpassword");

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().Be("Incorrect password!");
        }

        // ✅ TEST CASE 3: Xảy ra exception
        [Fact]
        public async Task PasswordAuthenticate_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                            .ThrowsAsync(new Exception("unexpected error"));

            // Act
            var result = await _controller.AuthenticatePassword("testuser", "password");

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().Be("Internal server error");
        }
    }
}
