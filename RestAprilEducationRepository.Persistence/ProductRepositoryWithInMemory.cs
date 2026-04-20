using RestAprilEducationRepository.Application.Products;
using RestAprilEducationRepository.Domain;

namespace RestAprilEducationRepository.Persistence;

public class ProductRepositoryWithInMemory : IProductRepository
{
    private static readonly List<Product> _products =
    [
        new() { Id = 1, Name = "Kalem", Price = 10, Barcode = "BRC001" },
        new() { Id = 2, Name = "Defter", Price = 25, Barcode = "BRC002" },
        new() { Id = 3, Name = "Silgi", Price = 5, Barcode = "BRC003" }
    ];

    private static int _nextId = 4;

    public Task<List<Product>> GetAllAsync()
    {
        return Task.FromResult(_products.ToList());
    }

    public Task<Product> CreateAsync(Product product)
    {
        product.Id = _nextId++;
        _products.Add(product);
        return Task.FromResult(product);
    }

    public Task<bool> AnyAsync(string productName)
    {
        var exists = _products.Any(p => p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(exists);
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<Product> UpdateAsync(Product product)
    {
        var index = _products.FindIndex(p => p.Id == product.Id);

        if (index is -1)
            throw new Exception($"Id({product.Id}) ile ürün bulunamadı");

        _products[index] = product;
        return Task.FromResult(product);
    }

    public Task DeleteAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);

        if (product is null)
            throw new Exception($"Id({id}) ile ürün bulunamadı");

        _products.Remove(product);
        return Task.CompletedTask;
    }

    public Task<List<Product>> GetAllWithPagedAsync(int pageNumber, int pageSize)
    {
        var pagedProducts = _products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult(pagedProducts);
    }
}
