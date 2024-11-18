using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Bmb.Domain.Core.Interfaces;
using Bmb.Orders.Domain.Contracts;
using FIAP.TechChallenge.ByteMeBurger.Persistence.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence;

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static void ConfigurePersistenceApp(this IServiceCollection services, IConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration.GetConnectionString("MySql")))
        {
            services.AddSingleton<IOrderRepository, InMemoryOrderRepository>()
                .AddSingleton<IProductRepository>(_ => new InMemoryProductRepository([]));
        }
        else
        {
            services.AddScoped<IDbConnection>(_ =>
            {
                DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
                var providerFactory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
                var conn = providerFactory.CreateConnection();
                conn.ConnectionString = configuration.GetConnectionString("MySql");
                conn.Open();
                return conn;
            });

            services.AddScoped<IOrderRepository, OrderRepositoryDapper>()
                .AddScoped<IProductRepository, ProductRepositoryDapper>();
        }
    }
}
