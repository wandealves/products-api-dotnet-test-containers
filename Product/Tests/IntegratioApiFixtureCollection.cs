using Entities;
using Newtonsoft.Json;

namespace Tests;

[CollectionDefinition(nameof(IntegrationApiFixtureCollection))]
public class IntegrationApiFixtureCollection : IClassFixture<IntegrationTestsFixture<Program>> { }

public class IntegrationTestsFixture<TProgram> : IDisposable, IAsyncLifetime where TProgram : class
{
  private BaseApiFactory<TProgram> _factory = null!;
  public HttpClient _client = null!;

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

  public HttpClient GetClient()
  {
    return _client;
  }

  public async Task InitializeAsync()
  {
    _factory = new BaseApiFactory<TProgram>();
    await _factory.StartDatabaseAsync();
    _client = _factory.CreateClient();
    _client.BaseAddress = new Uri("https://localhost");
  }

  public void Dispose()
  {
    _client.Dispose();
    _factory.Dispose();
  }

  public async Task DisposeAsync()
  {
    await _factory.StopDatabaseAsync();
  }
}
