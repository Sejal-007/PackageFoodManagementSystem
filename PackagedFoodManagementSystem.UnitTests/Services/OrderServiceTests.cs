using Moq;
using NUnit.Framework;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using System;

namespace PackagedFoodManagementSystem.UnitTests.Services
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _orderRepo;
        private ApplicationDbContext _context;
        private OrderService _service;

        [SetUp]
        public void Setup()
        {
            _orderRepo = new Mock<IOrderRepository>();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderTestDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _service = new OrderService(_context, _orderRepo.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void CreateOrder_Throws_WhenCartEmpty()
        {
            Assert.Throws<Exception>(() => _service.CreateOrder(1, "addr"));
        }

        [Test]
        public void PlaceOrder_DelegatesToRepo()
        {
            var order = new Order { CustomerId = 1, CreatedByUserID = 1, OrderStatus = "Pending", TotalAmount = 0 };
            _orderRepo.Setup(r => r.AddOrder(order));
            _orderRepo.Setup(r => r.Save());

            _service.PlaceOrder(order);

            _orderRepo.Verify(r => r.AddOrder(order), Times.Once);
            _orderRepo.Verify(r => r.Save(), Times.Once);
        }

        [Test]
        public void UpdateOrderStatus_UpdatesAndSaves()
        {
            var o = new Order { OrderID = 5, OrderStatus = "Pending" };
            _orderRepo.Setup(r => r.GetOrderById(5)).Returns(o);
            _orderRepo.Setup(r => r.UpdateOrder(o));
            _orderRepo.Setup(r => r.Save());

            _service.UpdateOrderStatus(5, "Completed");

            Assert.AreEqual("Completed", o.OrderStatus);
            _orderRepo.Verify(r => r.UpdateOrder(o), Times.Once);
            _orderRepo.Verify(r => r.Save(), Times.Once);
        }

        [Test]
        public void CancelOrder_Throws_WhenPreparingOrDispatched()
        {
            var o = new Order { OrderID = 7, OrderStatus = "Preparing" };
            _orderRepo.Setup(r => r.GetOrderById(7)).Returns(o);

            Assert.Throws<Exception>(() => _service.CancelOrder(7));
        }
    }
}
