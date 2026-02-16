using Moq;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Implementations;

[TestFixture]
public class ProductServiceTests
{
    private Mock<IProductRepository> _mockRepo;
    private ProductService _service;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IProductRepository>();
        _service = new ProductService(_mockRepo.Object);
    }

    [Test]
    public void CreateProduct_CallsRepoAddAndSave()
    {
        // Arrange
        var product = new Product { ProductName = "Milk", Category = "Dairy" };

        // Act
        _service.CreateProduct(product);

        // Assert
        _mockRepo.Verify(r => r.AddProduct(product), Times.Once);
        _mockRepo.Verify(r => r.Save(), Times.Once);
    }

    [Test]
    public void DeleteProduct_ProductExists_CallsRemove()
    {
        // Arrange
        var product = new Product { ProductId = 1, ProductName = "Milk", Category = "Dairy" };
        _mockRepo.Setup(r => r.GetProductById(1)).Returns(product);

        // Act
        _service.DeleteProduct(1);

        // Assert
        _mockRepo.Verify(r => r.RemoveProduct(product), Times.Once);
        _mockRepo.Verify(r => r.Save(), Times.Once);
    }
}