using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra;

public class ProductMap : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> builder)
  {
    // Configura a chave primária
    builder.HasKey(p => p.Id);

    // Define o nome da tabela
    builder.ToTable("Products");

    // Configura a coluna para a propriedade Name
    builder.Property(p => p.Name)
        .HasMaxLength(100) // Tamanho máximo de 100 caracteres
        .IsRequired();     // Define que o campo é obrigatório

    // Configura a coluna para a propriedade Price
    builder.Property(p => p.Price)
        .HasColumnType("decimal(18,2)"); // Define o tipo da coluna no banco de dados
  }
}