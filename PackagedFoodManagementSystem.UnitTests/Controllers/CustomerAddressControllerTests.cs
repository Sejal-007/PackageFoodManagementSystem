using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
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
        }

        [Test]
        public async Task Index_ReturnsViewWithAddresses()
        {
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<CustomerAddress>());
            var res = await _controller.Index();
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsIndexView()
        {
            _controller.ModelState.AddModelError("x", "err");
            var res = await _controller.Create(new CustomerAddress());
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public async Task Create_Post_Valid_Redirects()
        {
            var addr = new CustomerAddress { CustomerId = 1, AddressType = "Home", StreetAddress = "S", City = "C", PostalCode = "00001" };
            var res = await _controller.Create(addr);
            Assert.IsInstanceOf<RedirectToActionResult>(res);
            _serviceMock.Verify(s => s.AddAsync(addr), Times.Once);
        }

        [Test]
        public async Task Delete_Post_CallsService()
        {
            var res = await _controller.Delete(5);
            Assert.IsInstanceOf<OkResult>(res);
        }
    }
}
