using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

[TestFixture]
public class StoreManager1ControllerTests
{
    private Mock<IProductService> _mockProductService;
    private Mock<ICategoryService> _mockCategoryService;
    private StoreManager1Controller _controller;

    [SetUp]
    public void Setup()
    {
        _mockProductService = new Mock<IProductService>();
        _mockCategoryService = new Mock<ICategoryService>();

        _controller = new StoreManager1Controller(_mockProductService.Object, _mockCategoryService.Object);

        // Setup TempData (needed because the controller uses TempData["SuccessMessage"])
        var tempDataProvider = Mock.Of<ITempDataProvider>();
        var tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
        var tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());
        _controller.TempData = tempData;
    }

    [Test]
    public void Index_ReturnsViewWithAllProducts()
    {
        // Arrange
        var products = new List<Product> { new Product { ProductId = 1, ProductName = "Bread", Category = "Bakery" } };
        _mockProductService.Setup(s => s.GetAllProducts()).Returns(products);

        // Act
        var result = _controller.Index() as ViewResult;

        // Assert
        Assert.That(result.Model, Is.EqualTo(products));
    }

    [Test]
    public async Task Create_WithImageFile_ConvertsToBase64AndSaves()
    {
        // Arrange
        var product = new Product { ProductName = "Cake", Category = "Sweets" };

        // Mocking an image file
        var content = "fake image content";
        var fileName = "test.png";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(stream.Length);

        // Act
        var result = await _controller.Create(product, fileMock.Object) as RedirectToActionResult;

        // Assert
        Assert.That(product.ImageData, Does.StartWith("data:image/png;base64,"));
        _mockProductService.Verify(s => s.CreateProduct(product), Times.Once);
        Assert.That(_controller.TempData["SuccessMessage"], Is.EqualTo("Product added successfully!"));
        Assert.That(result.ActionName, Is.EqualTo("Index"));
    }

    [Test]
    public async Task AddProduct_Post_ResolvesCategoryIdFromName()
    {
        // Arrange
        var categories = new List<Category> { new Category { CategoryId = 10, CategoryName = "Organic" } };
        _mockCategoryService.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

        var product = new Product { ProductName = "Apple", Category = "Organic", CategoryId = 0 };

        // Act
        await _controller.AddProduct(product);

        // Assert
        Assert.That(product.CategoryId, Is.EqualTo(10));
    }

    [Test]
    public void Edit_Get_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        _mockProductService.Setup(s => s.GetProductById(99)).Returns((Product)null);

        // Act
        var result = _controller.Edit(99);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void Delete_Post_RedirectsToIndexWithDeleteMessage()
    {
        // Act
        var result = _controller.Delete(1) as RedirectToActionResult;

        // Assert
        _mockProductService.Verify(s => s.DeleteProduct(1), Times.Once);
        Assert.That(_controller.TempData["DeleteMessage"], Is.EqualTo("Product deleted successfully!"));
        Assert.That(result.ActionName, Is.EqualTo("Index"));
    }
}