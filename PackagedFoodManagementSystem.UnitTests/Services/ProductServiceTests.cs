using Moq;
using NUnit.Framework;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;

namespace PackagedFoodManagementSystem.UnitTests.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _repoMock;
        private ProductService _service;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _service = new ProductService(_repoMock.Object);
        }

        [Test]
        public void GetAllProducts_Delegates()
        {
            _repoMock.Setup(r => r.GetAllProducts()).Returns(new List<Product>());
            var res = _service.GetAllProducts();
            Assert.IsNotNull(res);
        }

        [Test]
        public void CreateNewProduct_AddsAndSaves()
        {
            var p = new Product { ProductId = 1, ProductName = "A", Price = 10, Category = "C" };
            _repoMock.Setup(r => r.AddProduct(p));
            _repoMock.Setup(r => r.Save());

            _service.CreateNewProduct(p);

            _repoMock.Verify(r => r.AddProduct(p), Times.Once);
            _repoMock.Verify(r => r.Save(), Times.Once);
        }

        [Test]
        public void UpdateProduct_UpdatesAndSaves()
        {
            var p = new Product { ProductId = 2, ProductName = "B", Price = 15, Category = "C" };
            _repoMock.Setup(r => r.UpdateProduct(p));
            _repoMock.Setup(r => r.Save());

            _service.UpdateProduct(p);

            _repoMock.Verify(r => r.UpdateProduct(p), Times.Once);
            _repoMock.Verify(r => r.Save(), Times.Once);
        }

        [Test]
        public void DeleteProduct_RemovesWhenExists()
        {
            var p = new Product { ProductId = 3, ProductName = "X", Category = "C", Price = 5 };
            _repoMock.Setup(r => r.GetProductById(3)).Returns(p);
            _repoMock.Setup(r => r.RemoveProduct(p));
            _repoMock.Setup(r => r.Save());

            _service.DeleteProduct(3);

            _repoMock.Verify(r => r.RemoveProduct(p), Times.Once);
            _repoMock.Verify(r => r.Save(), Times.Once);
        }
    }
}
