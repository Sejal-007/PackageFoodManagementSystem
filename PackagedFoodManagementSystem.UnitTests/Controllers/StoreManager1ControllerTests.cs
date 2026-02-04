using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class StoreManager1ControllerTests
    {
        private Mock<IProductService> _serviceMock;
        private StoreManager1Controller _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IProductService>();
            _controller = new StoreManager1Controller(_serviceMock.Object);
        }

        [Test]
        public void Index_ReturnsView()
        {
            _serviceMock.Setup(s => s.GetAllProducts()).Returns(new System.Collections.Generic.List<Product>());
            var res = _controller.Index();
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public void AddProduct_Get_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.AddProduct());

        [Test]
        public void Create_Post_WithImage_Redirects()
        {
            var product = new Product { ProductName = "A", Category = "C", Price = 1 };
            var ms = new MemoryStream(new byte[] { 1, 2, 3 });
            var file = new FormFile(ms, 0, ms.Length, "data", "img.png");

            var res = _controller.Create(product, file);
            Assert.IsInstanceOf<RedirectToActionResult>(res);
        }

        [Test]
        public void Edit_Get_ReturnsNotFound_WhenNull()
        {
            _serviceMock.Setup(s => s.GetProductById(9)).Returns((Product)null);
            var res = _controller.Edit(9);
            Assert.IsInstanceOf<NotFoundResult>(res);
        }

        [Test]
        public void Edit_Post_ReturnsRedirect_WhenValid()
        {
            var p = new Product { ProductId = 2, ProductName = "B", Category = "C", Price = 2, Quantity = 1 };
            _serviceMock.Setup(s => s.GetProductById(2)).Returns(p);
            var res = _controller.Edit(p, null);
            Assert.IsInstanceOf<RedirectToActionResult>(res);
            _serviceMock.Verify(s => s.UpdateProduct(It.IsAny<Product>()), Times.Once);
        }

        [Test]
        public void Delete_Post_CallsService()
        {
            var res = _controller.Delete(3);
            Assert.IsInstanceOf<RedirectToActionResult>(res);
            _serviceMock.Verify(s => s.DeleteProduct(3), Times.Once);
        }
    }
}
