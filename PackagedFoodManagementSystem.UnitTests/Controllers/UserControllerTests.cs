using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PackagedFoodFrontend.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Models;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController _controller;
        private Mock<ICustomerAddressService> _addressMock;
        private Mock<IOrderService> _orderMock;
        private Mock<ICustomerService> _customerMock;
        private Mock<IWalletService> _walletMock;

        [SetUp]
        public void Setup()
        {
            _addressMock = new Mock<ICustomerAddressService>();
            _orderMock = new Mock<IOrderService>();
            _customerMock = new Mock<ICustomerService>();
            _walletMock = new Mock<IWalletService>();

            _controller = new UserController(_addressMock.Object, _orderMock.Object, _customerMock.Object, _walletMock.Object);

            var httpContext = new DefaultHttpContext { Session = new TestMockSession() };
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        }

        [Test]
        public async Task Dashboard_ReturnsView_WithValidSession()
        {
            _controller.HttpContext.Session.SetInt32("UserId", 1);
            _orderMock.Setup(s => s.CountOrdersByUserAsync(1)).ReturnsAsync(5);
            _walletMock.Setup(s => s.GetByUserId(1)).Returns(new Wallet { Balance = 100 });

            var result = await _controller.Dashboard();
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task DeliveryAddress_ReturnsFilteredAddresses()
        {
            _controller.HttpContext.Session.SetInt32("UserId", 1);
            _customerMock.Setup(s => s.GetByUserIdAsync(1)).ReturnsAsync(new Customer { CustomerId = 10 });

            var addresses = new List<CustomerAddress> { 
                // Fix CS0117: StreetAddress matches your model, AddressLine1 did not
                new CustomerAddress { CustomerId = 10, StreetAddress = "Test St", AddressType = "Home", City = "City", PostalCode = "123" }
            };
            _addressMock.Setup(s => s.GetAllAsync()).ReturnsAsync(addresses);

            var result = await _controller.DeliveryAddress() as ViewResult;
            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        public void Profile_ReturnsView() // Sync method, no await (Fixes CS1061)
        {
            var result = _controller.EditProfile();
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }
    }
}