using Moq;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Services.Implementations;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _mockUserRepo;
    private Mock<ICustomerRepository> _mockCustRepo;
    private UserService _service;

    [SetUp]
    public void Setup()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockCustRepo = new Mock<ICustomerRepository>();
        _service = new UserService(_mockUserRepo.Object, _mockCustRepo.Object);
    }

    //[Test]
    //public async Task CreateUserAsync_SavesUserAndLinkedCustomer()
    //{
    //    // Act
    //    await _service.CreateUserAsync("John", "12345", "john@test.com", "password123", "User");

    //    // Assert: Verify both repositories were called
    //    _mockUserRepo.Verify(r => r.AddAsync(It.IsAny<UserAuthentication>(), It.IsAny<CancellationToken>()), Times.Once);
    //    _mockCustRepo.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
    //    _mockUserRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    //}
}