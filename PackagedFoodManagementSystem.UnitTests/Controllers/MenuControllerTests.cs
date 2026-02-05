using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Linq;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class MenuControllerTests
    {
        private Mock<IProductService> _serviceMock;
        private MenuController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IProductService>();
            _controller = new MenuController(_serviceMock.Object);
        }

        [Test]
        public void Index_ReturnsAllProducts_WhenNoCategory()
        {
            var list = new List<Product> { new Product { ProductName = "A", Category = "C" } };
            _serviceMock.Setup(s => s.GetAllProducts()).Returns(list);
            var res = _controller.Index(null);
            Assert.IsInstanceOf<ViewResult>(res);
        }

        public void Index_FiltersByCategory_WhenProvided()
        {
            Index_FiltersByCategory_WhenProvided(_controller);
        }

        [Test]
        public void Index_FiltersByCategory_WhenProvided(MenuController _controller)
        {
            var list = new List<Product>
            {
                new Product { ProductName = "A", Category = "C1" },
                new Product { ProductName = "B", Category = "C2" }
            };
            _serviceMock.Setup(s => s.GetAllProducts()).Returns(list);
            var res = _controller.Index("C2") as ViewResult;
            Assert.IsNotNull(res);
            var model = res.Model as IEnumerable<Product>;
            Assert.IsTrue(model.All(p => p.Category == "C2"));
        }
    }
}
