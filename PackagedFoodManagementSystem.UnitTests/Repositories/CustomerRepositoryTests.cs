using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Models;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class CustomerRepositoryTests
{
    private ApplicationDbContext _context;
    private CustomerRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CustomerTestDB_" + System.Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new CustomerRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetByIdAsync_ReturnsCustomerWithAddresses()
    {
        // Arrange
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Test",
            Email = "test@example.com",
            Phone = "123-456-7890",
            Addresses = new List<CustomerAddress> { new CustomerAddress { AddressId = 1, AddressType = "Home", StreetAddress = "Home", City = "TestCity", PostalCode = "12345" } }
        };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Addresses, Is.Not.Null);
        Assert.That(result.Addresses.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task DeleteAsync_RemovesCustomer()
    {
        // Arrange
        var customer = new Customer { CustomerId = 5, Name = "TestUser", Email = "test@example.com", Phone = "123-456-7890", Addresses = new List<CustomerAddress>() };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(5);

        // Assert
        var result = await _context.Customers.FindAsync(5);
        Assert.That(result, Is.Null);
    }
}