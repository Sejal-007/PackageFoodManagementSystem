using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackagedFoodManagementSystem.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PackagedFoodManagementSystem.UnitTests.TestHelpers;
using System.Security.Claims;
using System.Collections.Generic;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IConfiguration> _configMock;
        private HomeController _controller;
        private TestSession _session;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _configMock = new Mock<IConfiguration>();
            _controller = new HomeController(_userServiceMock.Object, _configMock.Object);

            var httpContext = new DefaultHttpContext();
            _session = new TestSession();
            httpContext.Session = _session;
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
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
        public async Task SignUp_Post_InvalidModel_ReturnsView()
        {
            _controller.ModelState.AddModelError("x", "err");
            var res = await _controller.SignUp(new UserAuthentication());
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public async Task AdminDashboard_SetsViewBag()
        {
            _userServiceMock.Setup(u => u.CountUsersByRoleAsync("User", default)).ReturnsAsync(5);
            _userServiceMock.Setup(u => u.CountUsersByRoleAsync("StoreManager", default)).ReturnsAsync(2);
            var res = await _controller.AdminDashboard();
            Assert.IsInstanceOf<ViewResult>(res);
            Assert.AreEqual(5, _controller.ViewBag.TotalCustomers);
            Assert.AreEqual(2, _controller.ViewBag.TotalStoreManagers);
        }

        [Test]
        public async Task Users_ReturnsViewWithUsers()
        {
            _userServiceMock.Setup(u => u.GetAllUsersAsync()).ReturnsAsync(new System.Collections.Generic.List<UserAuthentication>());
            var res = await _controller.Users();
            Assert.IsInstanceOf<ViewResult>(res);
        }
    }
}
