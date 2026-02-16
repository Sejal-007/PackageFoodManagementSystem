using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Models;

[TestFixture]
public class ProductRepositoryTests
{
    private ApplicationDbContext _context;
    private ProductRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ProductDb")
            .Options;
        _context = new ApplicationDbContext(options);
        _repository = new ProductRepository(_context);
    }

    [Test]
    public void UpdateProduct_DetachesLocalAndSetsModified()
    {
        // Arrange
        var product = new Product { ProductId = 10, ProductName = "Old Name", Category = "Test" };
        _context.Products.Add(product);
        _context.SaveChanges();

        // Simulate a new instance of the same product coming from a form
        var updatedProduct = new Product { ProductId = 10, ProductName = "New Name", Category = "Test" };

        // Act
        _repository.UpdateProduct(updatedProduct);
        _repository.Save();

        // Assert
        var result = _context.Products.Find(10);
        Assert.That(result.ProductName, Is.EqualTo("New Name"));
    }
}