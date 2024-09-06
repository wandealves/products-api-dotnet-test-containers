using Entities;

namespace Services;

public interface IProductService
{
  Task<IEnumerable<Product>> GetProductsAsync();
  Task<Product?> GetProductAsync(Guid id);
  Task<Product> CreateProductAsync(Product product);
  Task UpdateProductAsync(Product product);
  Task DeleteProductAsync(Guid id);
}
