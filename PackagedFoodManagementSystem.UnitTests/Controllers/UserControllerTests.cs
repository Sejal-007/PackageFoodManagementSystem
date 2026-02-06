using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

using PackagedFoodFrontend.Controllers;

using PackageFoodManagementSystem.Repository.Data;

using PackageFoodManagementSystem.Repository.Models;

using PackageFoodManagementSystem.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;

using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Tests

{

    [TestFixture]

    public class UserControllerTests

    {

        private Mock<ICustomerAddressService> _addressServiceMock;

        private ApplicationDbContext _context;

        private UserController _controller;

        private DefaultHttpContext _httpContext;

        [SetUp]

        public void Setup()

        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()

                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())

                .Options;

            _context = new ApplicationDbContext(options);

            _addressServiceMock = new Mock<ICustomerAddressService>();

            _controller = new UserController(_addressServiceMock.Object, _context);

            // FIXED: Initialize HttpContext AND Session immediately

            _httpContext = new DefaultHttpContext();

            _httpContext.Session = new MockSession(); // Assign here to prevent InvalidOperationException

            _controller.ControllerContext = new ControllerContext

            {

                HttpContext = _httpContext

            };

        }

        [TearDown]

        public void TearDown()

        {

            _controller?.Dispose();

            _context?.Dispose();

        }

        [Test]

        public async Task Dashboard_Redirects_IfSessionUserIdIsNull()

        {

            // Arrange: Ensure UserId is not in session

            _httpContext.Session.Clear();

            // Act

            var result = await _controller.Dashboard() as RedirectToActionResult;

            // Assert

            Assert.That(result?.ActionName, Is.EqualTo("SignIn"));

            Assert.That(result?.ControllerName, Is.EqualTo("Home"));

        }

        [Test]

        public async Task Dashboard_ReturnsView_WithCorrectData()

        {

            // Arrange

            int testUserId = 1;

            _httpContext.Session.SetInt32("UserId", testUserId);

            _httpContext.Session.SetString("UserName", "Test User");

            // Fix CS9035: Include all required fields from your models

            _context.Orders.Add(new Order

            {

                OrderID = 1,

                CreatedByUserID = testUserId,

                OrderStatus = "Placed",

                DeliveryAddress = "123 Tech Park",

                CustomerId = 1

            });

            await _context.SaveChangesAsync();

            // Act

            var result = await _controller.Dashboard() as ViewResult;

            // Assert

            Assert.That(result, Is.Not.Null);

            Assert.That(_controller.ViewBag.TotalOrders, Is.EqualTo(1));

            Assert.That(_controller.ViewBag.FullName, Is.EqualTo("Test User"));

        }

    }

    // Keep your MockSession implementation below

    public class MockSession : ISession

    {

        private readonly Dictionary<string, byte[]> _sessionStorage = new();

        public bool IsAvailable => true;

        public string Id => System.Guid.NewGuid().ToString();

        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public void Clear() => _sessionStorage.Clear();

        public Task CommitAsync(System.Threading.CancellationToken ct) => Task.CompletedTask;

        public Task LoadAsync(System.Threading.CancellationToken ct) => Task.CompletedTask;

        public void Remove(string key) => _sessionStorage.Remove(key);

        public void Set(string key, byte[] value) => _sessionStorage[key] = value;

        public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);

    }

}
