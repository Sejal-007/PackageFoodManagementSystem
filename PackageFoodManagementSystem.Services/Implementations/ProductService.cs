using PackageFoodManagementSystem.Repository;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;

namespace PackageFoodManagementSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _repo.GetAll();
        }

        public void CreateProduct(Product product)
        {
            _repo.Add(product);
            _repo.Save();
        }

        public void UpdateProduct(Product product)
        {
            _repo.Update(product);
            _repo.Save();
        }

        public void RemoveProduct(int id)
        {
            _repo.Delete(id);
            _repo.Save();
        }
    }
}