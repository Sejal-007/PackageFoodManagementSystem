using Moq;
using NUnit.Framework;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PackagedFoodManagementSystem.UnitTests.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        private Mock<IUserRepository> _userRepoMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepoMock.Object);
        }

        [Test]
        public async Task CreateUserAsync_ShouldCreateAndReturnId()
        {
            UserAuthentication created = null;
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<UserAuthentication>(), It.IsAny<CancellationToken>()))
                .Callback<object, CancellationToken>((u, ct) => created = u as UserAuthentication)
                .Returns(Task.CompletedTask);
            _userRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var id = await _userService.CreateUserAsync("Name", "9999", "e@e.com", "pwd");

            Assert.IsNotNull(created);
            Assert.AreEqual("Name", created.Name);
            Assert.AreEqual("e@e.com", created.Email);
            Assert.AreEqual("User", created.Role);
            Assert.AreEqual(created.Id, id);
        }

        [Test]
        public async Task AuthenticateAsync_ReturnsUser_WhenPasswordMatches()
        {
            var u = new UserAuthentication { Id = 2, Email = "a@a.com", Password = PackageFoodManagementSystem.Services.Helpers.PasswordHelper.HashPassword("pwd") };
            _userRepoMock.Setup(r => r.GetUserByEmailAsync("a@a.com")).ReturnsAsync(u);

            var res = await _userService.AuthenticateAsync("a@a.com", "pwd");
            Assert.IsNotNull(res);
            Assert.AreEqual(2, res.Id);
        }

        [Test]
        public async Task AuthenticateAsync_ReturnsNull_WhenWrongPassword()
        {
            var u = new UserAuthentication { Id = 3, Email = "b@b.com", Password = PackageFoodManagementSystem.Services.Helpers.PasswordHelper.HashPassword("pwd") };
            _userRepoMock.Setup(r => r.GetUserByEmailAsync("b@b.com")).ReturnsAsync(u);

            var res = await _userService.AuthenticateAsync("b@b.com", "bad");
            Assert.IsNull(res);
        }

        [Test]
        public async Task RegisterUserAsync_Succeeds_WhenRepositoryWorks()
        {
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<UserAuthentication>())).Returns(Task.CompletedTask);
            _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var user = new UserAuthentication { Name = "n", Email = "n@n.com", Password = "p" };
            var (s, e) = await _userService.RegisterUserAsync(user);
            Assert.IsTrue(s);
            Assert.IsNull(e);
        }

        [Test]
        public async Task RegisterUserAsync_ReturnsFalse_OnException()
        {
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<UserAuthentication>())).ThrowsAsync(new System.Exception("db"));
            var user = new UserAuthentication { Name = "n", Email = "n@n.com", Password = "p" };
            var (s, e) = await _userService.RegisterUserAsync(user);
            Assert.IsFalse(s);
            Assert.IsNotNull(e);
        }

        [Test]
        public async Task UpdateUserAsync_ReturnsFalse_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((UserAuthentication?)null);
            var res = await _userService.UpdateUserAsync(new UserAuthentication { Id = 99 });
            Assert.IsFalse(res.Success);
        }

        [Test]
        public async Task DeleteUserAsync_DelegatesRepository()
        {
            _userRepoMock.Setup(r => r.DeleteUserAsync(5)).Returns(Task.CompletedTask);
            _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            await _userService.DeleteUserAsync(5);
            _userRepoMock.Verify(r => r.DeleteUserAsync(5), Times.Once);
            _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CountUsersByRoleAsync_Delegates()
        {
            _userRepoMock.Setup(r => r.CountByRoleAsync("User", It.IsAny<CancellationToken>())).ReturnsAsync(7);
            var c = await _userService.CountUsersByRoleAsync("User");
            Assert.AreEqual(7, c);
        }

        [Test]
        public async Task GetAdminDashboardStatsAsync_Delegates()
        {
            _userRepoMock.Setup(r => r.GetDashboardStatsAsync()).ReturnsAsync((5,2,3));
            var stats = await _userService.GetAdminDashboardStatsAsync();
            Assert.AreEqual(5, stats.TotalCustomers);
            Assert.AreEqual(2, stats.TotalStoreManagers);
            Assert.AreEqual(3, stats.TotalOrders);
        }
    }
}
