using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id); // Needed to find the item before editing
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        IEnumerable<Product> GetMenuForCustomer();
        void CreateNewProduct(Product product);

        // Match this name to your ProductService implementation
        void RemoveProduct(int id);
        void DeleteProduct(int id); // Changed name to match standard controller naming
    }
}