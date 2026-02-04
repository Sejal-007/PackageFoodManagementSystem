using NUnit.Framework;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using System.Threading.Tasks;
using System.Linq;

namespace PackagedFoodManagementSystem.UnitTests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private ApplicationDbContext _context;
        private UserRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("UserRepoDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new UserRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddAndGetUser_Works()
        {
            var u = new UserAuthentication { Name = "A", Email = "a@a.com", Password = "p", MobileNumber = "123", Role = "User" };
            await _repo.AddAsync(u);
            await _repo.SaveChangesAsync();

            var byEmail = await _repo.GetByEmailAsync("a@a.com");
            Assert.IsNotNull(byEmail);
            var all = await _repo.GetAllAsync();
            Assert.IsTrue(all.Any());
        }

        [Test]
        public async Task CountByRole_Works()
        {
            _context.UserAuthentications.Add(new UserAuthentication { Name = "U1", Email = "u1@u.com", Password = "p", MobileNumber = "1", Role = "User" });
            _context.UserAuthentications.Add(new UserAuthentication { Name = "M1", Email = "m1@m.com", Password = "p", MobileNumber = "2", Role = "StoreManager" });
            await _context.SaveChangesAsync();

            var count = await _repo.CountByRoleAsync("User");
            Assert.AreEqual(1, count);
        }
    }
}
