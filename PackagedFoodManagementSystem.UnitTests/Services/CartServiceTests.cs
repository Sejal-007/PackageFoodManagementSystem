using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;

[TestFixture]
public class CartServiceTests
{
    private Mock<ICartRepository> _mockRepo;
    private CartService _service;
    private const int UserId = 1;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<ICartRepository>();
        _service = new CartService(_mockRepo.Object);
    }

    [Test]
    public async Task GetActiveCartAsync_WhenNoCartExists_CreatesNewCart()
    {
        // Arrange: Repository returns null for active cart
        _mockRepo.Setup(r => r.GetActiveCartByUserIdAsync(UserId)).ReturnsAsync((Cart)null);

        // Act
        var result = await _service.GetActiveCartAsync(UserId);

        // Assert
        Assert.Multiple(() => {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserAuthenticationId, Is.EqualTo(UserId));
            Assert.That(result.IsActive, Is.True);
        });
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Cart>()), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.AtLeastOnce);
    }

    [Test]
    public void AddItem_ExistingProduct_IncrementsQuantity()
    {
        // Arrange
        var productId = 10;
        var existingItem = new CartItem { ProductId = productId, Quantity = 2 };
        var existingCart = new Cart
        {
            CartId = 1,
            CartItems = new List<CartItem> { existingItem }
        };

        _mockRepo.Setup(r => r.GetActiveCartByUserIdAsync(UserId)).ReturnsAsync(existingCart);

        // Act
        _service.AddItem(UserId, productId);

        // Assert
        Assert.That(existingItem.Quantity, Is.EqualTo(3));
        _mockRepo.Verify(r => r.UpdateAsync(existingCart), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void DecreaseItem_WhenQuantityIsOne_RemovesItem()
    {
        // Arrange
        var productId = 10;
        var item = new CartItem { ProductId = productId, Quantity = 1 };
        var cart = new Cart { CartItems = new List<CartItem> { item } };

        _mockRepo.Setup(r => r.GetActiveCartByUserIdAsync(UserId)).ReturnsAsync(cart);

        // Act
        _service.DecreaseItem(UserId, productId);

        // Assert
        _mockRepo.Verify(r => r.RemoveItemAsync(item), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}