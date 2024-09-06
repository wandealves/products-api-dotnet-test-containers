using System.Net;
using Entities;
using FluentAssertions;

namespace Tests;

[Collection(nameof(IntegrationApiFixtureCollection))]
public class ProductTests
{
  private readonly IntegrationTestsFixture<Program> _fixture;

  public ProductTests(IntegrationTestsFixture<Program> fixture)
  {
    _fixture = fixture;
  }


  [Fact(DisplayName = "Criar novo produto")]
  [Trait("Categoria", "Produto")]
  public async Task Get_ShouldReturnProduct_WhenProductExists()
  {
    // Arrange
    var product = await _fixture.CreateProduct();
    var client = _fixture.GetClient();
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
    var client = _fixture.GetClient();
    // Act
    var response = await client.PostAsync("/api/products", product.ToStringContent());
    await response.Content.ReadAsStringAsync();
    // Assert
    response.Content.Should().NotBeNull();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }
}