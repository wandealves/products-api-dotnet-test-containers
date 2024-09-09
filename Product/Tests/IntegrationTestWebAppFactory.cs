using DotNet.Testcontainers.Builders;
using Infra;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace Tests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
  private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
      .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
      .WithPassword("1q2w3e4r@#$!")
      .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
      .Build();

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.UseEnvironment("Testing");
    builder.ConfigureTestServices(services =>
    {
      var descriptor = services.SingleOrDefault(
              d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
      if (descriptor is not null)
      {
        services.Remove(descriptor);
      }
      services.AddDbContext<AppDbContext>(options =>
            {
              var connectionString = _dbContainer.GetConnectionString();
              options.UseSqlServer(connectionString);
            });
    });
  }

  public async Task InitializeAsync()
  {
    await _dbContainer.StartAsync();
  }

  public async new Task DisposeAsync()
  {
    await _dbContainer.StopAsync();
  }
}