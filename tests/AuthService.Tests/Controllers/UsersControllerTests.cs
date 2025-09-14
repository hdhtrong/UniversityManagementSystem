using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AuthService.API.Controllers;
using AuthService.Domain.Entities;
using AuthService.Models;


namespace AuthService.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<ILogger<UsersController>> _loggerMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userManagerMock = MockUserManager<AppUser>();
            _loggerMock = new Mock<ILogger<UsersController>>();

            _controller = new UsersController(_userManagerMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((AppUser)null);

            var result = await _controller.GetUserById("nonexistent-id");

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUserDto_WhenUserExists()
        {
            var user = new AppUser { Id = "1", Email = "test@example.com", DisplayName = "Test User" };
            var roles = new List<string> { "Admin", "Manager" };

            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(roles as IList<string>);

            var result = await _controller.GetUserById("1");
            var okResult = result.Result as OkObjectResult;

            okResult.Should().NotBeNull();
            var dto = okResult.Value as UserDto;
            dto.Email.Should().Be("test@example.com");
            dto.Fullname.Should().Be("Test User");
            dto.Roles.Should().BeEquivalentTo(roles);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Email", "Required");
            var userDto = new UserDto();

            // Act
            var result = await _controller.Create(userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }
    }
}