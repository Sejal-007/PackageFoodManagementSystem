using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackagedFoodManagementSystem.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Repository.Data; // Added this
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PackagedFoodManagementSystem.UnitTests.TestHelpers;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IConfiguration> _configMock;
        private ApplicationDbContext _dbContext; // Real In-Memory DB for DB calls
        private HomeController _controller;
        private TestSession _session;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _configMock = new Mock<IConfiguration>();

            // Since HomeController uses ApplicationDbContext for CountAsync and ListAsync,
            // the easiest way to test it is using an In-Memory Database.
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _dbContext = new ApplicationDbContext(options);

            // HomeController expects: IUserService, IConfiguration, Context, DB
            // Providing _dbContext twice because your controller has two fields for it.
            _controller = new HomeController(
                _userServiceMock.Object,
                _configMock.Object,
                _dbContext,
                _dbContext
            );

            var httpContext = new DefaultHttpContext();
            _session = new TestSession();
            httpContext.Session = _session;
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public void SignIn_Get_ReturnsView()
        {
            var res = _controller.SignIn();
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public void Welcome_RedirectsWhenAuthenticated()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "x") }, "test"));
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            var res = _controller.Welcome();
            Assert.IsInstanceOf<RedirectToActionResult>(res);
        }

        [Test]
        public async Task SignIn_Post_InvalidCredentials_ReturnsView()
        {
            _userServiceMock.Setup(u => u.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((UserAuthentication)null);
            var res = await _controller.SignIn("a@a.com", "bad");
            Assert.IsInstanceOf<ViewResult>(res);
            Assert.IsFalse(_controller.ModelState.IsValid);
        }

        [Test]
        public void SignUp_Get_ReturnsView()
        {
            var res = _controller.SignUp();
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public async Task AdminDashboard_SetsViewBag()
        {
            // Seed the In-Memory DB since the controller calls _db.UserAuthentications directly
            _dbContext.UserAuthentications.Add(new UserAuthentication { Name = "U1", Role = "User", Email = "u1@test.com", Password = "123", MobileNumber = "123" });
            _dbContext.UserAuthentications.Add(new UserAuthentication { Name = "M1", Role = "StoreManager", Email = "m1@test.com", Password = "123", MobileNumber = "123" });
            await _dbContext.SaveChangesAsync();

            var res = await _controller.AdminDashboard();

            Assert.IsInstanceOf<ViewResult>(res);
            Assert.AreEqual(1, _controller.ViewBag.TotalCustomers);
            Assert.AreEqual(1, _controller.ViewBag.TotalStoreManagers);
        }

        [Test]
        public async Task Users_ReturnsViewWithUsers()
        {
            _dbContext.UserAuthentications.Add(new UserAuthentication { Name = "U1", Email = "u1@test.com", Password = "123", MobileNumber = "123" });
            await _dbContext.SaveChangesAsync();

            var res = await _controller.Users();
            var viewResult = res as ViewResult;
            var model = viewResult.Model as List<UserAuthentication>;

            Assert.IsInstanceOf<ViewResult>(res);
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
        }
    }
}