using NUnit.Framework;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using System.Linq;

namespace PackagedFoodManagementSystem.UnitTests.Repositories
{
    [TestFixture]
    public class BillRepositoryTests
    {
        private ApplicationDbContext _context;
        private BillRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("BillRepoDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new BillRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void AddAndGetBill_Works()
        {
            var b = new Bill { OrderID = 1, BillDate = System.DateTime.Now, SubtotalAmount = 0, FinalAmount = 0, BillingStatus = "Generated" };
            _repo.AddBill(b);
            _repo.Save();

            var got = _repo.GetBillByOrderId(1);
            Assert.IsNotNull(got);
            Assert.AreEqual(1, got.OrderID);
        }
    }
}
