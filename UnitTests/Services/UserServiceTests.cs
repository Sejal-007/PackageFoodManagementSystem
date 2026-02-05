using Moq;
using NUnit.Framework;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace PackageFoodManagementSystem.UnitTests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private UserService _userService;

n        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepoMock.Object);
        }

n        [Test]
        public async Task CreateUserAsync_ShouldHashPassword_AndReturnId()
        {
            UserAuthentication createdUser = null;
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<UserAuthentication>(), It.IsAny<CancellationToken>()))
                .Callback<object, CancellationToken>((u, ct) => createdUser = u as UserAuthentication)
                .Returns(Task.CompletedTask);

n            _userRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

n            var id = await _userService.CreateUserAsync("Test", "99999", "a@a.com", "pass123");

n            Assert.IsNotNull(createdUser);
            Assert.AreEqual("Test", createdUser.Name);
            Assert.AreEqual("a@a.com", createdUser.Email);
            Assert.AreNotEqual("pass123", createdUser.Password);
            Assert.AreEqual("User", createdUser.Role);
            Assert.AreEqual(createdUser.Id, id);
        }

n        [Test]
        public async Task GetUserByEmailAsync_ReturnsUser()
        {
            var user = new UserAuthentication { Id = 5, Email = "b@b.com" };
            _userRepoMock.Setup(r => r.GetByEmailAsync("b@b.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

n            var res = await _userService.GetUserByEmailAsync("b@b.com");
            Assert.AreEqual(5, res.Id);
        }

n        [Test]
        public async Task CountUsersByRoleAsync_DelegatesToRepo()
        {
            _userRepoMock.Setup(r => r.CountByRoleAsync("User", It.IsAny<CancellationToken>())).ReturnsAsync(10);
            var count = await _userService.CountUsersByRoleAsync("User");
            Assert.AreEqual(10, count);
        }
    }
}
