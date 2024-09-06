using Infra;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using DotNet.Testcontainers.Builders;

namespace Tests;
public class BaseApiFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
  private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
      .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
      .WithPassword("1q2w3e4r@#$!")
      .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433)) // Aguarda a porta do SQL Server estar pronta
      .Build();

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.UseEnvironment("Testing");
    builder.ConfigureTestServices(services =>
    {
      // Remove a configuração de contexto existente, se presente
      var descriptor = services.SingleOrDefault(
              d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
      if (descriptor != null)
      {
        services.Remove(descriptor);
      }
      // Adiciona o novo contexto apontando para o contêiner
      services.AddDbContext<AppDbContext>(options =>
          {
            var connectionString = _dbContainer.GetConnectionString();
            options.UseSqlServer(connectionString);
          });
    });
  }

  public async Task StartDatabaseAsync()
  {
    await _dbContainer.StartAsync();
  }

  public async Task StopDatabaseAsync()
  {
    await _dbContainer.StopAsync();
  }

}
