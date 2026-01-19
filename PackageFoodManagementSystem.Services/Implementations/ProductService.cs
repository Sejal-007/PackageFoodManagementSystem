using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;

namespace PackageFoodManagementSystem.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        // Fixes StoreManager1Controller and MenuController errors
        public IEnumerable<Product> GetAllProducts() => _repo.GetAllProducts();
        public void CreateProduct(Product product) => CreateNewProduct(product);

        // Fixes ProductController errors
        public IEnumerable<Product> GetMenuForCustomer() => _repo.GetAllProducts();
        public void CreateNewProduct(Product product)
        {
            _repo.AddProduct(product);
            _repo.Save();
        }

        public void UpdateProduct(Product product) { /* later */ }
        public void RemoveProduct(int id) { /* later */ }
    }
}