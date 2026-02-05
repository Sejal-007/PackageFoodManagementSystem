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
    public class CustomerAddressRepositoryTests
    {
        private ApplicationDbContext _context;
        private CustomerAddressRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CARRepoDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new CustomerAddressRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddAndGetAddress_Works()
        {
            var a = new CustomerAddress { CustomerId = 1, AddressType = "Home", StreetAddress = "S", City = "C", PostalCode = "0001", Landmark = "L" };
            await _repo.AddAsync(a);

            var all = await _repo.GetAllAsync();
            Assert.IsTrue(all.Any());

            var got = await _repo.GetByIdAsync(a.AddressId);
            Assert.IsNotNull(got);
        }
    }
}
