// File: Services/Interfaces/IProductService.cs
using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void RemoveProduct(int id);
    }
}