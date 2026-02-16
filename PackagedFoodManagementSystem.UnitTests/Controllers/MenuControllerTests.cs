using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class MenuControllerTests
{
    private Mock<IProductService> _mockProductService;
    private Mock<ICartService> _mockCartService;
    private MenuController _controller;

    [SetUp]
    public void Setup()
    {
        _mockProductService = new Mock<IProductService>();
        _mockCartService = new Mock<ICartService>();
        _controller = new MenuController(_mockProductService.Object, _mockCartService.Object);
    }

    [Test]
    public void Index_NoFilters_ReturnsAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { ProductId = 1, ProductName = "Apple", Category = "Fruit" },
            new Product { ProductId = 2, ProductName = "Bread", Category = "Bakery" }
        };
        _mockProductService.Setup(s => s.GetAllProducts()).Returns(products);

        // Act
        var result = _controller.Index(null, null) as ViewResult;
        var model = result.Model as IEnumerable<Product>;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(model.Count(), Is.EqualTo(2));
    }

    [Test]
    public void Index_WithSearchTerm_FiltersProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { ProductId = 1, ProductName = "Apple", Category = "Fruit" },
            new Product { ProductId = 2, ProductName = "Banana", Category = "Fruit" }
        };
        _mockProductService.Setup(s => s.GetAllProducts()).Returns(products);

        // Act
        var result = _controller.Index(null, "Apple") as ViewResult;
        var model = result.Model as IEnumerable<Product>;

        // Assert
        Assert.That(model.Count(), Is.EqualTo(1));
        Assert.That(model.First().ProductName, Is.EqualTo("Apple"));
        Assert.That(_controller.ViewBag.CurrentSearch, Is.EqualTo("Apple"));
    }

    [Test]
    public void Index_WithCategory_FiltersProducts()
    {
        // Arrange
        var products = new List<Product>
    {
        // Added Category to every Product because it is 'required' in your model
        new Product { ProductId = 1, ProductName = "Apple", Category = "Fruit" },
        new Product { ProductId = 2, ProductName = "Carrot", Category = "Vegetable" }
    };
        _mockProductService.Setup(s => s.GetAllProducts()).Returns(products);

        // Act
        var result = _controller.Index("Fruit", null) as ViewResult;
        var model = result.Model as IEnumerable<Product>;

        // Assert
        Assert.That(model.Count(), Is.EqualTo(1));
        Assert.That(model.First().Category, Is.EqualTo("Fruit"));
        Assert.That(_controller.ViewBag.SelectedCategory, Is.EqualTo("Fruit"));
    }

    [Test]
    public void Details_ProductExists_ReturnsViewWithProduct()
    {
        // 1. Arrange
        var product = new Product
        {
            ProductId = 1,
            ProductName = "Apple",
            Category = "Fruit" // satisfy 'required' constraint
        };
        _mockProductService.Setup(s => s.GetProductById(1)).Returns(product);

        // 2. Act
        // REMOVED 'await' because MenuController.Details is synchronous
        var result = _controller.Details(1) as ViewResult;

        // 3. Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Model, Is.EqualTo(product));
    }

    [Test]
    public void Details_ProductDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockProductService.Setup(s => s.GetProductById(999)).Returns((Product)null);

        // Act
        var result = _controller.Details(999);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }
}