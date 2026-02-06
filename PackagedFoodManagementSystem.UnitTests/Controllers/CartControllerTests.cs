using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class CartControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<ICartService> _cartService;
        private CartController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CartCtrlDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _cartService = new Mock<ICartService>();
            _controller = new CartController(_context, _cartService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void MyBasket_ReturnsViewWithCart_WhenHasCart()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "1") }, "test"));
            var http = new DefaultHttpContext { User = user };
            _controller.ControllerContext = new ControllerContext { HttpContext = http };

            _context.Carts.Add(new Cart { UserAuthenticationId = 1, IsActive = true, CartItems = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 1 } } });
            _context.SaveChanges();

            var res = _controller.MyBasket();
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public void Add_Post_CallsService()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
        new Claim(ClaimTypes.NameIdentifier, "1")
    }));
            var http = new DefaultHttpContext { User = user };
            _controller.ControllerContext = new ControllerContext { HttpContext = http };

            // Act
            var res = _controller.Add(5);

            // Assert
            // FIX: Changed from OkResult to JsonResult to match actual return type
            Assert.IsInstanceOf<JsonResult>(res);

            // Verify service call
            _cartService.Verify(s => s.AddItem(1, 5), Times.Once);
        }

        [Test]
        public void Decrease_Post_CallsService()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
        new Claim(ClaimTypes.NameIdentifier, "1")
    }));
            var http = new DefaultHttpContext { User = user };
            _controller.ControllerContext = new ControllerContext { HttpContext = http };

            // Act
            var res = _controller.Decrease(5);

            // Assert
            // FIX: Changed from OkResult to JsonResult to match actual return type
            Assert.IsInstanceOf<JsonResult>(res);

            // Verify service call
            _cartService.Verify(s => s.DecreaseItem(1, 5), Times.Once);
        }

        [Test]
        public void GetItemQty_ReturnsZero_WhenNoCart()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "2") }, "test"));
            var http = new DefaultHttpContext { User = user };
            _controller.ControllerContext = new ControllerContext { HttpContext = http };

            var res = _controller.GetItemQty(10);
            Assert.IsInstanceOf<JsonResult>(res);
        }

        [Test]
        public void Remove_Post_CallsService()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "1") }, "test"));
            var http = new DefaultHttpContext { User = user };
            _controller.ControllerContext = new ControllerContext { HttpContext = http };

            var req = new PackageFoodManagementSystem.Application.DTOs.CartRequest { ProductId = 3 };
            var res = _controller.Remove(req);
            Assert.IsInstanceOf<OkResult>(res);
            _cartService.Verify(s => s.Remove(1, 3), Times.Once);
        }
    }
}
