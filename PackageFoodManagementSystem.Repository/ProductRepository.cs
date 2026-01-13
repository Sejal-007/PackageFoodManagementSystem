using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) => _context = context;

        public IEnumerable<Product> GetAll() => _context.Products.ToList();

        public Product GetById(int id) => _context.Products.Find(id);

        public void Add(Product product) => _context.Products.Add(product);

        public void Update(Product product) => _context.Products.Update(product);

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null) _context.Products.Remove(product);
        }

        public void Save() => _context.SaveChanges();
    }
}