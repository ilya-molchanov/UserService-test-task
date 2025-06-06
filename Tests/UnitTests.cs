using Moq;
using Microsoft.Extensions.Logging;
using TestBackend.ServiceLibrary.Repositories.Interfaces;
using AutoMapper;
using TestBackend.Internal.BusinessObjects;
using TestBackend.Internal.Enums;
using TestBackend.ServiceLibrary.Models;
using BackendTest.Internal.Exceptions;
using BackendTestWebAPI.Application.Services;
using TestBackend.ServiceLibrary.Enums;

namespace Tests
{
    [TestClass]
    public sealed class UnitTests
    {
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly Mock<IUsersRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _userService;
        private readonly CreateUserDto _userDto;
        private readonly SqlUser _user;

        public UnitTests()
        {
            _mockUserRepository = new Mock<IUsersRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UserService>>();

            _userDto = new CreateUserDto
            {
                Name = "Test User",
                Email = "test@test.com",
                Password = "1234567",
                Role = UserRoleCode.User
            };

            _user = new SqlUser { Id = 1, Name = _userDto.Name, Email = _userDto.Email, Password = BCrypt.Net.BCrypt.HashPassword(_userDto.Password), Role = 1 };

            _userService = new UserService(_mockUserRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task CreateUserAsync_ShouldReturnSuccess_WhenUserIsCreated()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.DoesUserExistByEmailAsync(_userDto.Email))
                .ReturnsAsync(false);

            _mockUserRepository.Setup(r => r.CreateUserAsync(_user, new CancellationToken()))
                .ReturnsAsync(_user.Id);

            _mockMapper.Setup(r => r.Map<SqlUser>(_userDto)).Returns(_user);

            // Act
            var result = await _userService.CreateUserAsync(_userDto);

            // Assert
            Assert.AreEqual(_user.Id, result);

            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<SqlUser>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public void CreateUserAsync_ShouldReturnFailed_WhenUserWithSuchEmailAlreadyExists()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.DoesUserExistByEmailAsync(_userDto.Email))
                .ReturnsAsync(true);

            _mockUserRepository.Setup(r => r.CreateUserAsync(_user, new CancellationToken()))
                .ReturnsAsync(_user.Id);

            _mockMapper.Setup(r => r.Map<SqlUser>(_userDto)).Returns(_user);

            // Act
            var exception = Assert.ThrowsExceptionAsync<InternalApiBusinessException>(
                () => _userService.CreateUserAsync(_userDto));

            // Assert
            Assert.AreEqual(exception.Result.InternalApiBusinessErrorCode, InternalApiErrorCodes.UserWithGivenEmailAlreadyExists);

            _mockUserRepository.Verify(r => r.CreateUserAsync(It.IsAny<SqlUser>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task CreateUserAsync_ShouldNotCreateUser_WhenInputIsInvalid()
        {
            // Arrange
            var invalidUserDto = new CreateUserDto
            {
                Name = "",
                Email = "invalid email",
                Password = "12346546",
                Role = UserRoleCode.Admin
            };

            _mockUserRepository.Setup(r => r.DoesUserExistByEmailAsync(_userDto.Email))
                .ReturnsAsync(false);

            // Act
            InternalApiBusinessException exception = await Assert.ThrowsExceptionAsync<InternalApiBusinessException>(() => _userService.CreateUserAsync(invalidUserDto));

            // Assert
            Assert.AreEqual("The user name is empty.", exception.Content.Message);

            _mockUserRepository.Verify(r => r.CreateUserAsync(It.IsAny<SqlUser>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdateUserRoleAsync_ShouldUpdateUserRole_WhenUserIsValid()
        {
            // Arrange
            var updatedUserDto = new UpdateUserRoleDto
            {
                Id = 1,
                Role = UserRoleCode.Admin
            };

            _mockUserRepository.Setup(r => r.GetUserByIdAsync(_user.Id))
                .ReturnsAsync(_user);

            _mockUserRepository.Setup(r => r.UpdateUserRoleAsync(_user.Id, (int)UserRoleCode.Admin, new CancellationToken()))
                .ReturnsAsync(ResultOperation.Success);

            // Act
            var result = await _userService.UpdateUserRoleAsync(updatedUserDto);

            // Assert
            Assert.AreEqual(ResultOperation.Success, result);

            _mockUserRepository.Verify(repo => repo.UpdateUserRoleAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task GetUsersAsync_ShouldReturnNames_WhenUsersExist()
        {
            // Arrange
            var users = new List<SqlUser> { _user };

            _mockUserRepository.Setup(r => r.GetUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _userService.GetUserNamesAsync();

            // Assert
            Assert.AreEqual(users.First().Name, result.ElementAt(0));

            _mockUserRepository.Verify(repo => repo.GetUsersAsync(), Times.Once);
        }
    }
}
