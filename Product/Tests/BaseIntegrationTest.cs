using Entities;
using Newtonsoft.Json;

namespace Tests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
  public HttpClient _client = null!;
  public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
  {
    _client = factory.CreateClient();
    _client.BaseAddress = new Uri("https://localhost");
  }

  public HttpClient GetClient()
  {
    return _client;
  }

  public async Task<Product> CreateProductAsync()
  {
    var product = new Product
    {
      Name = "Product 1",
      Price = 10
    };

    var client = GetClient();
    var response = await client.PostAsync("/api/products", product.ToStringContent());
    await response.Content.ReadAsStringAsync();
    var json = await response.Content.ReadAsStringAsync();
    var productCreated = JsonConvert.DeserializeObject<Product>(json);
    return productCreated ?? product;
  }

  public async Task<Product> GetProductAsync(HttpResponseMessage response)
  {
    var json = await response.Content.ReadAsStringAsync();
    var product = JsonConvert.DeserializeObject<Product>(json);
    return product ?? new Product();
  }

  public async Task<IEnumerable<Product>> GetProductsAsync(HttpResponseMessage response)
  {
    var json = await response.Content.ReadAsStringAsync();
    var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
    return products ?? Enumerable.Empty<Product>();
  }
}