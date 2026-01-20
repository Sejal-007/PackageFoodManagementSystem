// Ensure ProductService implements IProductService
public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _repo.GetAllProducts();
    }

    public void CreateProduct(Product product)
    {
        _repo.CreateProduct(product);
    }

    public void UpdateProduct(Product product)
    {
        _repo.UpdateProduct(product);
    }

    public void RemoveProduct(int id)
    {
        _repo.RemoveProduct(id);
    }
}