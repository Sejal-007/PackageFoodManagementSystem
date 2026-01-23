using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Interfaces; // Ensure this is included

namespace PackageFoodManagementSystem.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Product> GetAllProducts() => _repo.GetAllProducts();

        // Added this to help the Edit function find the specific product
        public Product GetProductById(int id) => _repo.GetProductById(id);

        public void CreateProduct(Product product) => CreateNewProduct(product);

        public IEnumerable<Product> GetMenuForCustomer() => _repo.GetAllProducts();

        public void CreateNewProduct(Product product)
        {
            _repo.AddProduct(product);
            _repo.Save();
        }

        // Logic for EDIT

        public void UpdateProduct(Product product)
        {
            _repo.UpdateProduct(product);
            _repo.Save(); // Crucial: Saves the edit to the database
        }

        // Logic for DELETE
        public void RemoveProduct(int id) => DeleteProduct(id);

        public void DeleteProduct(int id)
        {
            var product = _repo.GetProductById(id);
            if (product != null)
            {
                _repo.RemoveProduct(product);
                _repo.Save();
            }
        }
        //public void RemoveProduct(int id)
        //{
        //    var product = _repo.GetProductById(id);
        //    if (product != null)
        //    {
        //        _repo.RemoveProduct(product);
        //        _repo.Save(); // Crucial: Removes the row from the database
        //    }
        //}
    }
}