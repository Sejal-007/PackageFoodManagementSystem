using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Repository.Interfaces; // Ensure this is included

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

        // 1. Implementation to find a single product by ID
        public Product GetProductById(int id)
        {
            return _context.Products.Find(id);
        }

        // 2. Implementation to mark a product as updated
        public void UpdateProduct(Product product)
        {
            // Check if the object is already being tracked to avoid the "already tracked" error
            var local = _context.Products.Local.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (local != null)
            {
                _context.Entry(local).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }

            // Force Entity Framework to see this object as "Modified"
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        // 3. Implementation to remove a product
        public void RemoveProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public void Save() => _context.SaveChanges();
    }
}