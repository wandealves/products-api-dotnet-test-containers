using System.Net;
using Entities;
using FluentAssertions;

namespace Tests;

public class ProductTests : BaseIntegrationTest
{
  public ProductTests(IntegrationTestWebAppFactory factory) : base(factory)
  {

  }

  [Fact(DisplayName = "Obter todosprodutos")]
  [Trait("Categoria", "Produto")]
  public async Task GetAll_ShouldReturnProduct_WhenProductExists()
  {
    // Arrange
    var client = GetClient();
    // Act
    var response = await client.GetAsync("/api/products");
    await response.Content.ReadAsStringAsync();
    var products = await GetProductsAsync(response);
    // Assert
    products.Should().NotBeNullOrEmpty();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact(DisplayName = "Obter produto pelo Id")]
  [Trait("Categoria", "Produto")]
  public async Task Get_ShouldReturnProduct_WhenProductExists()
  {
    // Arrange
    var product = await CreateProductAsync();
    var client = GetClient();
    // Act
    var response = await client.GetAsync($"/api/products/{product.Id}");
    await response.Content.ReadAsStringAsync();
    // Assert
    response.Content.Should().NotBeNull();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact(DisplayName = "Criar novo produto")]
  [Trait("Categoria", "Produto")]
  public async Task Create_ShouldCreateProduct()
  {
    // Arrange
    var product = new Product
    {
      Name = "Product 1",
      Price = 10
    };
    var client = GetClient();
    // Act
    var response = await client.PostAsync("/api/products", product.ToStringContent());
    await response.Content.ReadAsStringAsync();
    // Assert
    response.Content.Should().NotBeNull();
    response.StatusCode.Should().Be(HttpStatusCode.Created);
  }

  [Fact(DisplayName = "Atualizar produto")]
  [Trait("Categoria", "Produto")]
  public async Task Update_ShouldThrow_WhenProductIsNull()
  {
    // Arrange
    var product = await CreateProductAsync();
    product.Name = "Product Updated";
    var client = GetClient();
    // Act
    var response = await client.PutAsync($"/api/products/{product.Id}", product.ToStringContent());
    await response.Content.ReadAsStringAsync();

    var responseGet = await client.GetAsync($"/api/products/{product.Id}");
    await responseGet.Content.ReadAsStringAsync();
    var productGet = await GetProductAsync(responseGet);
    // Assert
    response.Content.Should().NotBeNull();
    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    responseGet.Content.Should().NotBeNull();
    responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
    productGet.Should().NotBeNull();
    product.Name.Should().Be(productGet.Name);
  }

  [Fact(DisplayName = "Deletar produto")]
  [Trait("Categoria", "Produto")]
  public async Task Delete_ShouldDeleteProduct_WhenProductExists()
  {
    // Arrange
    var product = await CreateProductAsync();
    var client = GetClient();
    // Act
    var response = await client.DeleteAsync($"/api/products/{product.Id}");
    await response.Content.ReadAsStringAsync();
    // Assert
    response.Content.Should().NotBeNull();
    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
  }
}