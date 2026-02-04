using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Product GetProductById(int id);
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);

        // ADDED TO RESOLVE CS0535
        IEnumerable<Product> GetMenuForCustomer();
        void CreateNewProduct(Product product);
        void RemoveProduct(int id);
    }
}