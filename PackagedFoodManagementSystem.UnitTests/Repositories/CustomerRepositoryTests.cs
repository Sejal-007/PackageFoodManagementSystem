using NUnit.Framework;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PackagedFoodManagementSystem.UnitTests.Repositories
{
    [TestFixture]
    public class CustomerRepositoryTests
    {
        private ApplicationDbContext _context;
        private CustomerRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CustomerRepoDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new CustomerRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddAndGetCustomer_Works()
        {
            var c = new Customer { Name = "Cust", Email = "c@c.com", Phone = "99999", Status = "Active" };
            c.Addresses = new System.Collections.Generic.List<CustomerAddress> {
                new CustomerAddress { AddressType = "Home", StreetAddress = "S", City = "C", PostalCode = "0001", Landmark = "L" }
            };

            await _repo.AddAsync(c);

            var all = await _repo.GetAllAsync();
            Assert.IsTrue(all.Any());

            var got = await _repo.GetByIdAsync(c.CustomerId);
            Assert.IsNotNull(got);
            Assert.AreEqual("Cust", got.Name);
            Assert.IsNotNull(got.Addresses);
        }

        [Test]
        public async Task UpdateAndDelete_Works()
        {
            var c = new Customer { Name = "Cust2", Email = "c2@c.com", Phone = "88888", Status = "Active" };
            await _repo.AddAsync(c);

            c.Name = "Cust2Updated";
            await _repo.UpdateAsync(c);

            var got = await _repo.GetByIdAsync(c.CustomerId);
            Assert.AreEqual("Cust2Updated", got.Name);

            await _repo.DeleteAsync(c.CustomerId);
            var after = await _repo.GetByIdAsync(c.CustomerId);
            Assert.IsNull(after);
        }
    }
}
