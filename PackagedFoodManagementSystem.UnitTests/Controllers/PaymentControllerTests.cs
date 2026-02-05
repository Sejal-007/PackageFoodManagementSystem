using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackagedFoodFrontend.Controllers;
using PackageFoodManagementSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using System.Linq;
using PackageFoodManagementSystem.Application.Controllers;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private ApplicationDbContext _context;
        private PaymentController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("PaymentCtrlDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _controller = new PaymentController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void Payment_ReturnsView()
        {
            var res = _controller.Payment(5);
            Assert.IsInstanceOf<ViewResult>(res);
            Assert.AreEqual(5, _controller.ViewBag.OrderId);
        }

        [Test]
        public void Confirm_ReturnsBadRequest_WhenOrderNotFound()
        {
            var res = _controller.Confirm(99, "COD", null);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public void Confirm_CreatesPayment_WhenOrderExists()
        {
            _context.Orders.Add(new Order { OrderID = 1, CustomerId = 1, CreatedByUserID = 1, OrderStatus = "Pending", TotalAmount = 0 });
            // Add a corresponding Bill so PaymentController can create a Payment
            _context.Bills.Add(new Bill { OrderID = 1, BillDate = System.DateTime.Now, FinalAmount = 100m, BillingStatus = "Unpaid" });
            _context.SaveChanges();

            var res = _controller.Confirm(1, "COD", null);
            Assert.IsInstanceOf<RedirectToActionResult>(res);
            var pay = _context.Payments.FirstOrDefault(p => p.OrderID == 1);
            Assert.IsNotNull(pay);
        }
    }
}
