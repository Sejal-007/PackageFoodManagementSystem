using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Models;
using System.Threading.Tasks;

[TestFixture]
public class CustomerAddressRepositoryTests
{
    private ApplicationDbContext _context;
    private CustomerAddressRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "RepoTestDB_" + System.Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new CustomerAddressRepository(_context);
    }

    [Test]
    public async Task AddAsync_SavesAddressToDb()
    {
        // Arrange
        var address = new CustomerAddress { AddressId = 1, StreetAddress = "Test Road", AddressType = "Home", City = "TestCity", PostalCode = "12345", Landmark = "Near Park" };

        // Act
        await _repository.AddAsync(address);

        // Assert
        var result = await _context.CustomerAddresses.FindAsync(1);
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task DeleteAsync_RemovesAddressFromDb()
    {
        // Arrange
        var address = new CustomerAddress { AddressId = 99, StreetAddress = "Some Rd", AddressType = "Office", City = "CityX", PostalCode = "99999", Landmark = "Opp Mall" };
        _context.CustomerAddresses.Add(address);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(99);

        // Assert
        var result = await _context.CustomerAddresses.FindAsync(99);
        Assert.That(result, Is.Null);
    }
}