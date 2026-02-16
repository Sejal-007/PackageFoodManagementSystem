using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PackageFoodManagementSystem.Controllers; // Adjust namespace if needed
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Application.DTOs;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class CartControllerTests
    {
        private Mock<ICartService> _cartMock;
        private CartController _controller;
        private const int TestUserId = 1;

        [SetUp]
        public void Setup()
        {
            _cartMock = new Mock<ICartService>();
            _controller = new CartController(_cartMock.Object);

            // Mocking the Authenticated User Claim
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, TestUserId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task MyBasket_ReturnsViewWithCartData()
        {
            // Arrange
            var cart = new Cart { UserAuthenticationId = TestUserId, CartItems = new List<CartItem>() };
            _cartMock.Setup(s => s.GetActiveCartAsync(TestUserId)).ReturnsAsync(cart);

            // Act
            var result = await _controller.MyBasket();

            // Assert
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(cart));
        }

        [Test]
        public async Task Add_ValidProduct_ReturnsJsonWithUpdatedQty()
        {
            // Arrange
            int productId = 101;
            var cart = new Cart
            {
                CartItems = new List<CartItem> { new CartItem { ProductId = productId, Quantity = 2 } }
            };
            _cartMock.Setup(s => s.GetActiveCartAsync(TestUserId)).ReturnsAsync(cart);

            // Act
            var result = await _controller.Add(productId);

            // Assert
            _cartMock.Verify(s => s.AddItem(TestUserId, productId), Times.Once);
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public async Task Decrease_ValidProduct_ReturnsJsonWithUpdatedQty()
        {
            // Arrange
            int productId = 101;
            var cart = new Cart
            {
                CartItems = new List<CartItem> { new CartItem { ProductId = productId, Quantity = 1 } }
            };
            _cartMock.Setup(s => s.GetActiveCartAsync(TestUserId)).ReturnsAsync(cart);

            // Act
            var result = await _controller.Decrease(productId);

            // Assert
            _cartMock.Verify(s => s.DecreaseItem(TestUserId, productId), Times.Once);
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public async Task GetTotalItems_ReturnsCorrectSum()
        {
            // Arrange
            var cart = new Cart
            {
                CartItems = new List<CartItem>
                {
                    new CartItem { Quantity = 2 },
                    new CartItem { Quantity = 3 }
                }
            };
            _cartMock.Setup(s => s.GetActiveCartAsync(TestUserId)).ReturnsAsync(cart);

            // Act
            var result = await _controller.GetTotalItems();

            // Assert
            var jsonResult = result as JsonResult;
            Assert.That(jsonResult.Value, Is.EqualTo(5));
        }

        [Test]
        public void Remove_CallsServiceAndReturnsOk()
        {
            // Arrange
            var request = new CartRequest { ProductId = 101 };

            // Act
            var result = _controller.Remove(request);

            // Assert
            _cartMock.Verify(s => s.Remove(TestUserId, 101), Times.Once);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }
    }
}