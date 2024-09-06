using Microsoft.EntityFrameworkCore;
using Entities;

namespace Infra;

public sealed class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions options)
      : base(options)
  {
  }

  public DbSet<Product> Products { get; set; }

  // Aplica o mapeamento ao criar o modelo
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Aplica o mapeamento para a entidade Product
    modelBuilder.ApplyConfiguration(new ProductMap());
  }
}