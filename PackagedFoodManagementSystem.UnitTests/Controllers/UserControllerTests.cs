using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PackagedFoodFrontend.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using PackagedFoodManagementSystem.UnitTests.TestHelpers;
using System.Collections.Generic;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<ICustomerAddressService> _addrService;
        private ApplicationDbContext _context;
        private UserController _controller;
        private TestSession _session;

        [SetUp]
        public void Setup()
        {
            _addrService = new Mock<ICustomerAddressService>();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("UserCtrlDb")
                .Options;
            _context = new ApplicationDbContext(options);

            _controller = new UserController(_addrService.Object, _context);

            _session = new TestSession();
            var httpContext = new DefaultHttpContext();
            httpContext.Session = _session;
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Dashboard_RedirectsToSignIn_WhenNoSession()
        {
            // No UserId in session
            var res = await _controller.Dashboard();
            Assert.IsInstanceOf<RedirectToActionResult>(res);
        }

        [Test]
        public async Task DeliveryAddress_ReturnsUserAddresses()
        {
            _session.Set("UserId", System.Text.Encoding.UTF8.GetBytes("2"));
            _context.CustomerAddresses.Add(new CustomerAddress { AddressId = 1, CustomerId = 2, AddressType = "Home", StreetAddress = "S", City = "C", PostalCode = "00001", Landmark = "L" });
            _context.SaveChanges();

            _addrService.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<CustomerAddress> { new CustomerAddress { AddressId = 1, CustomerId = 2, AddressType = "Home", StreetAddress = "S", Landmark = "L", City = "C", PostalCode = "00001" } });

            var res = await _controller.DeliveryAddress();
            Assert.IsInstanceOf<ViewResult>(res);
            var view = res as ViewResult;
            Assert.IsNotNull(view.Model);
        }
    }
}
