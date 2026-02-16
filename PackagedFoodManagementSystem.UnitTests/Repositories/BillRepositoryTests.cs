using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Models;
using System.Linq;

[TestFixture]
public class BillRepositoryTests
{
    private ApplicationDbContext _context;
    private BillRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "BillingDB_" + System.Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new BillRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public void AddBill_ShouldAddBillToDatabase()
    {
        // Arrange
        var bill = new Bill { OrderID = 5, BillingStatus = "Pending" };

        // Act
        _repository.AddBill(bill);
        _repository.Save();

        // Assert
        var savedBill = _context.Bill.FirstOrDefault(b => b.OrderID == 5);
        Assert.That(savedBill, Is.Not.Null);
    }

    [Test]
    public void GetBillByOrderId_ReturnsCorrectBill()
    {
        // Arrange
        var bill = new Bill { OrderID = 20, BillingStatus = "Paid" };
        _context.Bill.Add(bill);
        _context.SaveChanges();

        // Act
        var result = _repository.GetBillByOrderId(20);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.OrderID, Is.EqualTo(20));
    }
}