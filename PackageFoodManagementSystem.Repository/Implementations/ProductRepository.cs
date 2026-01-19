using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Repository.Implementations

{

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts() => _context.Products.ToList();

        public void AddProduct(Product product) => _context.Products.Add(product);

        public void Save() => _context.SaveChanges();

        public Product GetProductById(int id)
        {
            throw new NotImplementedException();
        }
    }
}