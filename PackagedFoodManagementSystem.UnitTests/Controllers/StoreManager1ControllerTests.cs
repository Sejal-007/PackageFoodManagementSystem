using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Moq;

using NUnit.Framework;

using PackageFoodManagementSystem.Application.Controllers;

using PackageFoodManagementSystem.Repository.Models;

using PackageFoodManagementSystem.Services.Interfaces;

using System.Collections.Generic;

namespace PackageFoodManagementSystem.Tests

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

            // FIXED: Simplified TempData mock to resolve CS7036 and CS0246

            var httpContext = new DefaultHttpContext();

            var tempDataProvider = new Mock<ITempDataProvider>();

            _controller.TempData = new TempDataDictionary(httpContext, tempDataProvider.Object);

        }

        // FIXED: Added TearDown to resolve NUnit1032

        [TearDown]

        public void TearDown()

        {

            _controller?.Dispose();

        }

        [Test]

        public void Create_ValidProduct_RedirectsToIndex()

        {

            // Arrange

            // FIXED: Added required members to resolve CS9035

            var product = new Product

            {

                ProductId = 1,

                ProductName = "Fresh Milk",

                Category = "Dairy",

                Price = 2.99m

            };

            var fileMock = new Mock<IFormFile>();

            // Act

            var result = _controller.Create(product, fileMock.Object) as RedirectToActionResult;

            // Assert

            Assert.That(result?.ActionName, Is.EqualTo("Index"));

        }

    }

}
