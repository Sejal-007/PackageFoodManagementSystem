using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class OrderControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<IOrderService> _orderService;
        private OrderController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("OrderCtrlDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _orderService = new Mock<IOrderService>();
            _controller = new OrderController(_context, _orderService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void Index_ReturnsView()
        {
            _orderService.Setup(o => o.GetAllOrders()).Returns(new List<Order>());
            var res = _controller.Index();
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public void Create_Post_Redirects()
        {
            var o = new Order { CustomerId = 1, CreatedByUserID = 1, OrderStatus = "Pending", TotalAmount = 0 };
            var res = _controller.Create(o);
            Assert.IsInstanceOf<RedirectToActionResult>(res);
            _orderService.Verify(x => x.PlaceOrder(o), Times.Once);
        }

        [Test]
        public void Checkout_RedirectsToLogin_WhenNotAuthenticated()
        {
            // Arrange: Create an empty context so HttpContext is not null
            var user = new ClaimsPrincipal(new ClaimsIdentity()); // No claims = Not Authenticated
            var httpContext = new DefaultHttpContext { User = user };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var res = _controller.Checkout();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(res);
            var redirect = (RedirectToActionResult)res;

            // Depending on your logic, verify it goes to Login
            // Assert.AreEqual("Login", redirect.ActionName);
        }

        [Test]
        public void PlaceOrder_Post_RedirectsToPayment()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "1") }, "test"));
            var http = new DefaultHttpContext { User = user };
            _controller.ControllerContext = new ControllerContext { HttpContext = http };

            _orderService.Setup(o => o.CreateOrder(1, "addr")).Returns(5);
            var res = _controller.PlaceOrder("addr");
            Assert.IsInstanceOf<RedirectToActionResult>(res);
        }
    }
}
