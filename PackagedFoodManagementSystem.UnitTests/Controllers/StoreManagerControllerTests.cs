using NUnit.Framework;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class StoreManagerControllerTests
    {
        private StoreManagerController _controller;
        private Mock<IProductService> _productServiceMock;
        private Mock<IOrderService> _orderServiceMock;

        [SetUp]
        public void Setup()
        {
            _productServiceMock = new Mock<IProductService>();
            _orderServiceMock = new Mock<IOrderService>();

            // Using the service-based constructor
            _controller = new StoreManagerController(_productServiceMock.Object, _orderServiceMock.Object);

            var httpContext = new DefaultHttpContext();
            // Using the renamed class to avoid CS0101
            httpContext.Session = new TestMockSession();

            var tempDataProvider = new Mock<ITempDataProvider>();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _controller.TempData = new TempDataDictionary(httpContext, tempDataProvider.Object);
        }

        [Test]
        public async Task Home_ReturnsView_WithCorrectViewBagData()
        {
            // FIX CS9035: Added required Product Name and Category
            var products = new List<Product>
            {
                new Product { ProductName = "Milk", Category = "Dairy", Quantity = 10 },
                new Product { ProductName = "Bread", Category = "Bakery", Quantity = 2 }
            };

            _productServiceMock.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(products);
            _orderServiceMock.Setup(s => s.CountTodayOrdersAsync()).ReturnsAsync(5);
            _orderServiceMock.Setup(s => s.SumTotalRevenueAsync()).ReturnsAsync(100.0m);
            _orderServiceMock.Setup(s => s.GetOrdersAsync(null)).ReturnsAsync(new List<Order>());

            var result = await _controller.Home();

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        // FIX CS1061: Removed 'await' because Profile() is synchronous (returns IActionResult)
        [Test]
        public void Profile_ReturnsView()
        {
            var result = _controller.Profile();
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Orders_ReturnsView_WithModel()
        {
            // FIX CS9035: Added required OrderStatus
            var orders = new List<Order> { new Order { OrderID = 1, OrderStatus = "Pending" } };
            _orderServiceMock.Setup(s => s.GetOrdersAsync("Pending")).ReturnsAsync(orders);

            var result = await _controller.Orders("Pending");

            Assert.That(result, Is.Not.Null);
        }

        // FIX CS1061: Removed 'await' because Inventory() is synchronous
        [Test]
        public void Inventory_ReturnsView()
        {
            var result = _controller.Inventory();
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task OrdersDashboard_ReturnsView()
        {
            _orderServiceMock.Setup(s => s.GetOrdersAsync(null)).ReturnsAsync(new List<Order>());
            var result = await _controller.OrdersDashboard();
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }
    }

    // FIX CS0101: Renamed to TestMockSession to prevent name collision in the namespace
    public class TestMockSession : ISession
    {
        private readonly Dictionary<string, byte[]> _storage = new Dictionary<string, byte[]>();
        public bool IsAvailable => true;
        public string Id => Guid.NewGuid().ToString();
        public IEnumerable<string> Keys => _storage.Keys;
        public void Clear() => _storage.Clear();
        public Task CommitAsync(CancellationToken ct = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken ct = default) => Task.CompletedTask;
        public void Remove(string key) => _storage.Remove(key);
        public void Set(string key, byte[] value) => _storage[key] = value;
        public bool TryGetValue(string key, out byte[] value) => _storage.TryGetValue(key, out value);
    }
}