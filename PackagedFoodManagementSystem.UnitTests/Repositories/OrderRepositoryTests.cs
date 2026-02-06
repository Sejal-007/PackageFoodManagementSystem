using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

using PackageFoodManagementSystem.Repository.Data;

using PackageFoodManagementSystem.Repository.Implementations;

using PackageFoodManagementSystem.Repository.Models;

using System;

using System.Collections.Generic;

using System.Linq;

namespace PackageFoodManagementSystem.Tests

{

    [TestFixture]

    public class OrderRepositoryTests

    {

        private ApplicationDbContext _context;

        private OrderRepository _repository;

        [SetUp]

        public void Setup()

        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()

                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())

                .Options;

            _context = new ApplicationDbContext(options);

            _repository = new OrderRepository(_context);

        }

        [TearDown]

        public void TearDown()

        {

            _context.Database.EnsureDeleted();

            _context.Dispose();

        }

        private Order CreateValidOrder(int orderId)

        {

            return new Order

            {

                OrderID = orderId,

                CustomerId = 1,

                CreatedByUserID = 1,

                OrderStatus = "Pending",

                TotalAmount = 150.00m,

                DeliveryAddress = "123 Test Street",

                OrderDate = DateTime.Now

            };

        }

        [Test]

        public void AddOrder_PersistsToDatabase()

        {

            // Arrange

            var order = CreateValidOrder(1);

            // Act

            _repository.AddOrder(order);

            _repository.Save();

            // Assert

            var savedOrder = _context.Orders.Find(1);

            Assert.That(savedOrder, Is.Not.Null);

            Assert.That(savedOrder.OrderStatus, Is.EqualTo("Pending"));

        }

        [Test]

        public void GetOrderById_ReturnsCorrectOrder()

        {

            // Arrange

            var order1 = CreateValidOrder(101);

            var order2 = CreateValidOrder(102);

            _context.Orders.AddRange(order1, order2);

            _context.SaveChanges();

            // Act

            var result = _repository.GetOrderById(102);

            // Assert

            Assert.That(result, Is.Not.Null);

            Assert.That(result.OrderID, Is.EqualTo(102));

        }

        [Test]

        public void GetAllOrders_ReturnsCompleteList()

        {

            // Arrange

            _context.Orders.Add(CreateValidOrder(1));

            _context.Orders.Add(CreateValidOrder(2));

            _context.SaveChanges();

            // Act

            var result = _repository.GetAllOrders();

            // Assert

            Assert.That(result.Count(), Is.EqualTo(2));

        }

        [Test]

        public void UpdateOrder_ChangesValuesSuccessfully()

        {

            // Arrange

            var order = CreateValidOrder(1);

            _context.Orders.Add(order);

            _context.SaveChanges();

            // Act

            order.OrderStatus = "Shipped";

            _repository.UpdateOrder(order);

            _repository.Save();

            // Assert

            var updated = _context.Orders.Find(1);

            Assert.That(updated.OrderStatus, Is.EqualTo("Shipped"));

        }

        [Test]

        public void DeleteOrder_RemovesFromDatabase()

        {

            // Arrange

            var order = CreateValidOrder(1);

            _context.Orders.Add(order);

            _context.SaveChanges();

            // Act

            _repository.DeleteOrder(1);

            _repository.Save();

            // Assert

            var result = _context.Orders.Find(1);

            Assert.That(result, Is.Null);

        }

    }

}
