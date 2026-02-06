using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Moq;

using NUnit.Framework;

using PackageFoodManagementSystem.Application.Controllers;

using PackageFoodManagementSystem.Repository.Models;

using PackageFoodManagementSystem.Services.Interfaces;

using System.Collections.Generic;

using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Tests

{

    [TestFixture]

    public class CustomerAddressControllerTests

    {

        private Mock<ICustomerAddressService> _serviceMock;

        private CustomerAddressController _controller;

        [SetUp]

        public void Setup()

        {

            _serviceMock = new Mock<ICustomerAddressService>();

            _controller = new CustomerAddressController(_serviceMock.Object);

            // Mock TempData for the Create action

            var tempDataMock = new Mock<ITempDataDictionary>();

            _controller.TempData = tempDataMock.Object;

        }

        [TearDown]

        public void TearDown()

        {

            // Resolve NUnit1032

            _controller?.Dispose();

        }

        [Test]

        public async Task Index_ReturnsViewWithAddresses()

        {

            // Arrange

            var mockAddresses = new List<CustomerAddress> { new CustomerAddress { AddressId = 1, City = "Chennai" } };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(mockAddresses);

            // Act

            var result = await _controller.Index() as ViewResult;

            // Assert

            Assert.That(result, Is.Not.Null);

            Assert.That(result!.Model, Is.EqualTo(mockAddresses));

        }

        [Test]

        public async Task Create_ValidModel_RedirectsToIndex()

        {

            // Arrange

            var address = new CustomerAddress { City = "Mumbai", StreetAddress = "Main St", AddressType = "Home", PostalCode = "400001" };

            // Act

            var result = await _controller.Create(address) as RedirectToActionResult;

            // Assert

            _serviceMock.Verify(s => s.AddAsync(address), Times.Once);

            Assert.That(result!.ActionName, Is.EqualTo(nameof(_controller.Index)));

        }

        [Test]

        public async Task Create_InvalidModel_ReturnsIndexViewWithData()

        {

            // Arrange

            _controller.ModelState.AddModelError("City", "Required");

            var address = new CustomerAddress();

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<CustomerAddress>());

            // Act

            var result = await _controller.Create(address) as ViewResult;

            // Assert

            Assert.That(result!.ViewName, Is.EqualTo("Index"));

            _serviceMock.Verify(s => s.AddAsync(It.IsAny<CustomerAddress>()), Times.Never);

        }

        [Test]

        public async Task Delete_CallsServiceAndReturnsOk()

        {

            // Arrange

            int testId = 5;

            // Act

            var result = await _controller.Delete(testId);

            // Assert

            _serviceMock.Verify(s => s.DeleteAsync(testId), Times.Once);

            Assert.That(result, Is.TypeOf<OkResult>());

        }

    }

}
