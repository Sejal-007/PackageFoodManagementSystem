using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class MenuControllerTests
    {
        private Mock<IProductService> _serviceMock;
        private Mock<ICartService> _cartServiceMock; // New Mock
        private MenuController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IProductService>();
            _cartServiceMock = new Mock<ICartService>();

            // Pass both mocks to the constructor
            _controller = new MenuController(_serviceMock.Object, _cartServiceMock.Object);

            // Set up a fake user so GetUserId() doesn't crash the test
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task Index_ReturnsAllProducts_WhenNoCategory()
        {
            // Arrange
            var list = new List<Product> { new Product { ProductName = "A", Category = "C" } };
            _serviceMock.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(list);
            _cartServiceMock.Setup(c => c.GetActiveCartAsync(It.IsAny<int>())).ReturnsAsync(new Cart());

            // Act
            var res = await _controller.Index(null);

            // Assert
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public async Task Index_FiltersByCategory_WhenProvided()
        {
            // Arrange
            var list = new List<Product>
            {
                new Product { ProductName = "A", Category = "C1" },
                new Product { ProductName = "B", Category = "C2" }
            };
            _serviceMock.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(list);
            _cartServiceMock.Setup(c => c.GetActiveCartAsync(It.IsAny<int>())).ReturnsAsync(new Cart());

            // Act
            var res = await _controller.Index("C2") as ViewResult;

            // Assert
            Assert.IsNotNull(res);
            var model = res.Model as IEnumerable<Product>;
            Assert.AreEqual(1, model.Count());
            Assert.IsTrue(model.All(p => p.Category == "C2"));
        }
    }
}