using Entities;
using Infra;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class ProductService : IProductService
{
  private readonly AppDbContext _context;

  public ProductService(AppDbContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Product>> GetProductsAsync()
  {
    return await _context.Products.ToListAsync();
  }

  public async Task<Product?> GetProductAsync(Guid id)
  {
    return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
  }

  public async Task<Product> CreateProductAsync(Product product)
  {
    product.CreatedOnUtc = DateTime.UtcNow;
    _context.Products.Add(product);
    await _context.SaveChangesAsync();
    return product;
  }

  public async Task UpdateProductAsync(Product product)
  {
    _context.Products.Update(product);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteProductAsync(Guid id)
  {
    var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
    if (product != null)
    {
      _context.Products.Remove(product);
      await _context.SaveChangesAsync();
    }
  }
}