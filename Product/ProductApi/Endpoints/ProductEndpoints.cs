using Endpoints.Dtos;
using Entities;
using Services;

namespace Endpoints;
public static class ProductEndpoints
{
  public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
  {
    // GET: Retorna todos os produtos
    routes.MapGet("/api/products", async (IProductService service) =>
    {
      var products = await service.GetProductsAsync();
      return Results.Ok(products);
    });

    // GET: Retorna um produto pelo ID
    routes.MapGet("/api/products/{id:guid}", async (Guid id, IProductService service) =>
    {
      var product = await service.GetProductAsync(id);
      if (product == null)
        return Results.NotFound();

      return Results.Ok(product);
    });

    // POST: Cria um novo produto
    routes.MapPost("/api/products", async (ProductDto product, IProductService service) =>
    {
      var productCreated = await service.CreateProductAsync(new Product
      {
        Name = product.Name,
        Price = product.Price
      });
      return Results.Created($"/api/products/{productCreated.Id}", productCreated);
    });

    // PUT: Atualiza um produto existente
    routes.MapPut("/api/products/{id:guid}", async (Guid id, ProductDto updatedProduct, IProductService service) =>
    {
      var product = await service.GetProductAsync(id);
      if (product == null)
        return Results.NotFound();

      product.Name = updatedProduct.Name;
      product.Price = updatedProduct.Price;

      await service.UpdateProductAsync(product);

      return Results.NoContent();
    });

    // DELETE: Exclui um produto
    routes.MapDelete("/api/products/{id:guid}", async (Guid id, IProductService service) =>
    {
      var product = await service.GetProductAsync(id);
      if (product == null)
        return Results.NotFound();

      await service.DeleteProductAsync(product.Id);
      return Results.NoContent();
    });
  }
}