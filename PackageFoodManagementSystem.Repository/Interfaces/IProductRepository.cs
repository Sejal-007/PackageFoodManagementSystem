using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Repository.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product); // Add this
        void RemoveProduct(Product product); // Add this
        void Save();
    }
}