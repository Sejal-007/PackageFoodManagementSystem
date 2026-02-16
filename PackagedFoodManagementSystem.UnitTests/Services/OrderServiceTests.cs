//using Microsoft.EntityFrameworkCore;

//using Moq;

//using NUnit.Framework;

//using PackageFoodManagementSystem.Repository.Data;

//using PackageFoodManagementSystem.Repository.Interfaces;

//using PackageFoodManagementSystem.Repository.Models;

//using PackageFoodManagementSystem.Services.Implementations;

//using System;

//using System.Collections.Generic;

//using System.Linq;

//namespace PackageFoodManagementSystem.Test.Services

//{

//    [TestFixture]

//    public class OrderServiceTests

//    {

//        private ApplicationDbContext _context;

//        private Mock<IOrderRepository> _orderRepoMock;

//        private OrderService _service;

//        [SetUp]

//        public void Setup()

//        {

//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()

//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())

//                .Options;

//            _context = new ApplicationDbContext(options);

//            _orderRepoMock = new Mock<IOrderRepository>();

//            _service = new OrderService(_context, _orderRepoMock.Object);

//        }

//        [TearDown]

//        public void TearDown()

//        {

//            _context.Database.EnsureDeleted();

//            _context.Dispose();

//        }

//        [Test]

//        public void CreateOrder_ValidCart_CreatesOrderAndDeactivatesCart()

//        {

//            int userId = 1;

//            _context.Customers.Add(new Customer { CustomerId = 10, UserId = userId, Name = "User", Email = "a@b.com", Phone = "123" });

//            var product = new Product { ProductId = 50, ProductName = "Food", Category = "Meal", Price = 100m };

//            var cart = new Cart { CartId = 1, UserAuthenticationId = userId, IsActive = true };

//            cart.CartItems = new List<CartItem> { new CartItem { ProductId = 50, Product = product, Quantity = 2 } };

//            _context.Carts.Add(cart);

//            _context.SaveChanges();

//            int orderId = _service.CreateOrder(userId, "123 Main St");

//            var order = _context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.OrderID == orderId);

//            Assert.That(order, Is.Not.Null);

//            Assert.That(cart.IsActive, Is.False);

//        }

//        [Test]

//        public void UpdateOrderStatus_ValidOrder_AddsToHistory()

//        {

//            var order = new Order

//            {

//                OrderID = 5,

//                OrderStatus = "Pending",

//                DeliveryAddress = "123 Test St",

//                CustomerId = 1,

//                TotalAmount = 50m

//            };

//            _context.Orders.Add(order);

//            _context.SaveChanges();

//            _service.UpdateOrderStatus(5, "Shipped", "1", "Remark");

//            var history = _context.OrderStatusHistories.FirstOrDefault(h => h.OrderID == 5);

//            Assert.That(order.OrderStatus, Is.EqualTo("Shipped"));

//            Assert.That(history, Is.Not.Null);

//        }

//        [Test]

//        public void CancelOrder_DispatchedStatus_ThrowsException()

//        {

//            var order = new Order { OrderID = 1, OrderStatus = "Dispatched", DeliveryAddress = "Addr" };

//            _orderRepoMock.Setup(r => r.GetOrderById(1)).Returns(order);

//            var ex = Assert.Throws<Exception>(() => _service.CancelOrder(1));

//            Assert.That(ex.Message, Does.Contain("cannot be cancelled"));

//        }

//    }

//}
