using Moq;
using NUnit.Framework;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces; // Required for explicit casting
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace PackagedFoodManagementSystem.UnitTests.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<ICustomerRepository> _customerRepoMock;
        private UserService _userService;
        // Helper to access explicitly implemented interface methods
        private IUserService ServiceInterface => (IUserService)_userService;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _customerRepoMock = new Mock<ICustomerRepository>();

            // Fixes CS7036: Added the missing ICustomerRepository argument
            _userService = new UserService(_userRepoMock.Object, _customerRepoMock.Object);
        }

        [Test]
        public async Task CreateUserAsync_ShouldCreateAndReturnId()
        {
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<UserAuthentication>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _userRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _customerRepoMock.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                .Returns(Task.CompletedTask);

            var id = await _userService.CreateUserAsync("Name", "9999", "e@e.com", "pwd");

            Assert.AreEqual(0, id); // Current implementation returns user.Id (default 0 in mock)
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<UserAuthentication>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task AuthenticateAsync_Throws_WhenCalled()
        {
            // Fixes CS1061: Use ServiceInterface to access explicitly implemented AuthenticateAsync
            // Note: This will catch the NotImplementedException currently in your service logic
            Assert.ThrowsAsync<NotImplementedException>(async () =>
                await ServiceInterface.AuthenticateAsync("a@a.com", "pwd"));
        }

        [Test]
        public async Task RegisterUserAsync_Throws_WhenCalled()
        {
            var user = new UserAuthentication { Name = "n", Email = "n@n.com", Password = "p" };

            // Fixes CS1061: Explicit interface call
            Assert.ThrowsAsync<NotImplementedException>(async () =>
                await ServiceInterface.RegisterUserAsync(user));
        }

        [Test]
        public async Task DeleteUserAsync_Throws_WhenCalled()
        {
            // Fixes CS1061: Explicit interface call
            Assert.ThrowsAsync<NotImplementedException>(async () =>
                await ServiceInterface.DeleteUserAsync(5));
        }

        [Test]
        public async Task GetAdminDashboardStatsAsync_Throws_WhenCalled()
        {
            // Fixes CS1061: Explicit interface call
            Assert.ThrowsAsync<NotImplementedException>(async () =>
                await ServiceInterface.GetAdminDashboardStatsAsync());
        }

        [Test]
        public async Task CountUsersByRoleAsync_Delegates()
        {
            _userRepoMock.Setup(r => r.CountByRoleAsync("User", It.IsAny<CancellationToken>()))
                .ReturnsAsync(7);

            var c = await _userService.CountUsersByRoleAsync("User");

            Assert.AreEqual(7, c);
        }
    }
}