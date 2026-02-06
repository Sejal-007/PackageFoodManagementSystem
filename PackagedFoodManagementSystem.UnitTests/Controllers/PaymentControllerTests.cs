using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using PackagedFoodFrontend.Controllers;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Repository.Data;

using PackageFoodManagementSystem.Repository.Models;

using PackageFoodManagementSystem.Services.Implementations;

using System;

using System.Collections.Generic;

using System.Linq;

namespace PackageFoodManagementSystem.Tests

{

    [TestFixture]

    public class PaymentControllerTests

    {

        private ApplicationDbContext _context;

        private Mock<IOrderService> _orderServiceMock;

        private PaymentController _controller;

        private const int TestOrderId = 500;

        private const int TestUserId = 1;

        [SetUp]

        public void Setup()

        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()

                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())

                .Options;

            _context = new ApplicationDbContext(options);

            _orderServiceMock = new Mock<IOrderService>();

            _controller = new PaymentController(_context, _orderServiceMock.Object);

            // Seed required Customer with Phone property to avoid DbUpdateException

            _context.Customers.Add(new Customer

            {

                CustomerId = TestUserId,

                Name = "John Doe",

                Email = "john@test.com",

                Phone = "000-111-2222"

            });

            // Seed required Order with OrderStatus

            _context.Orders.Add(new Order

            {

                OrderID = TestOrderId,

                OrderStatus = "Placed",

                CustomerId = TestUserId,

                DeliveryAddress = "Test Address",

                OrderDate = DateTime.Now

            });

            // Seed required Bill

            _context.Bill.Add(new Bill

            {

                BillID = 1,

                OrderID = TestOrderId,

                FinalAmount = 150.00m,

                BillingStatus = "Unpaid",

                BillDate = DateTime.Now

            });

            _context.SaveChanges();

        }

        [TearDown]

        public void TearDown()

        {

            _controller?.Dispose();

            _context?.Database.EnsureDeleted();

            _context?.Dispose();

        }

        [Test]

        public void Confirm_ValidCard_UpdatesBillToPaidAndRedirectsSuccess()

        {

            // Act

            var result = _controller.Confirm(TestOrderId, "Card", "1234 5678 1234 5678") as RedirectToActionResult;

            // Assert

            var bill = _context.Bill.First(b => b.OrderID == TestOrderId);

            var payment = _context.Payment.FirstOrDefault(p => p.OrderID == TestOrderId);

            Assert.That(bill.BillingStatus, Is.EqualTo("Paid"));

            Assert.That(payment, Is.Not.Null);

            Assert.That(payment!.PaymentStatus, Is.EqualTo("Success"));

            Assert.That(result?.ActionName, Is.EqualTo("Success"));

            _orderServiceMock.Verify(s => s.UpdateOrderStatus(TestOrderId, "Confirmed", It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        }

        [Test]

        public void Confirm_DeclinedCard_RedirectsToFailure()

        {

            // Act: 16 zeros triggers simulation failure logic

            var result = _controller.Confirm(TestOrderId, "Card", "0000 0000 0000 0000") as RedirectToActionResult;

            // Assert

            Assert.That(result?.ActionName, Is.EqualTo("Failure"));

            Assert.That(result?.RouteValues!["orderId"], Is.EqualTo(TestOrderId));

            _orderServiceMock.Verify(s => s.UpdateOrderStatus(TestOrderId, "Payment Failed", It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        }

        [Test]

        public void Confirm_COD_SetsStatusToPending()

        {

            // Act

            var result = _controller.Confirm(TestOrderId, "COD", null!) as RedirectToActionResult;

            // Assert

            var payment = _context.Payment.First(p => p.OrderID == TestOrderId);

            var bill = _context.Bill.First(b => b.OrderID == TestOrderId);

            Assert.That(payment.PaymentStatus, Is.EqualTo("Pending"));

            Assert.That(bill.BillingStatus, Is.EqualTo("Unpaid")); // COD doesn't set bill to Paid immediately

            Assert.That(result?.ActionName, Is.EqualTo("Success"));

        }

        [Test]

        public void Payment_MissingId_ReturnsBadRequest()

        {

            // Act

            var result = _controller.Payment(0);

            // Assert

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

        }

    }

}
