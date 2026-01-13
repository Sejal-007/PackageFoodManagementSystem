
using PackageFoodManagementSystem.Repository.Models;

public interface IProductService
{
    void CreateProduct(Product product);
    IEnumerable<object> GetAllProducts();
}