using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductService> _serviceMock;
        private ProductController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IProductService>();
            _controller = new ProductController(_serviceMock.Object);
        }

        [Test]
        public void Index_ReturnsViewWithProducts()
        {
            _serviceMock.Setup(s => s.GetMenuForCustomer()).Returns(new List<Product>());
            var res = _controller.Index();
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public void Create_Get_ReturnsView()
        {
            var res = _controller.Create();
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public void Create_Post_InvalidModel_ReturnsView()
        {
            _controller.ModelState.AddModelError("x", "err");
            var res = _controller.Create(new Product { ProductName = "", Category = "", Price = 0, Quantity = 0 });
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public void Create_Post_Valid_Redirects()
        {
            var p = new Product { ProductName = "A", Category = "C", Price = 1 };
            var res = _controller.Create(p);
            Assert.IsInstanceOf<RedirectToActionResult>(res);
            _serviceMock.Verify(s => s.CreateNewProduct(p), Times.Once);
        }
    }
}
