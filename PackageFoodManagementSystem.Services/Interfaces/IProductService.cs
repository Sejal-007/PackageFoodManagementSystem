using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Services.Interfaces
{
    public interface IProductService
    {
        // Add these so the Controllers stop throwing errors
        IEnumerable<Product> GetAllProducts();
        void CreateProduct(Product product);

        // Keep these if your other ProductController uses them
        IEnumerable<Product> GetMenuForCustomer();
        void CreateNewProduct(Product product);

        void UpdateProduct(Product product);
        void RemoveProduct(int id);
    }
}