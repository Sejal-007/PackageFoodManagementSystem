using NUnit.Framework;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using System.Linq;

namespace PackagedFoodManagementSystem.UnitTests.Repositories
{
    [TestFixture]
    public class PaymentRepositoryTests
    {
        private ApplicationDbContext _context;
        private PaymentRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("PaymentRepoDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new PaymentRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void AddAndGetPayment_Works()
        {
            var p = new Payment { OrderID = 1, BillID = 1, PaymentMethod = "Cash", PaymentStatus = "Success", TransactionReference = "T1" };
            _repo.AddPayment(p);
            _repo.Save();

            var got = _repo.GetPaymentByBillId(1);
            Assert.IsNotNull(got);
            Assert.AreEqual(1, got.BillID);
        }
    }
}
