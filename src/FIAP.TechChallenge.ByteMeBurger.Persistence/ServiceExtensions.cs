// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
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
            .AddScoped<ICustomerRepository, CustomerRepositoryDapper>()
            .AddScoped<IProductRepository, ProductRepositoryDapper>()
            .AddScoped<IPaymentRepository, PaymentRepositoryDapper>();
    }
}
