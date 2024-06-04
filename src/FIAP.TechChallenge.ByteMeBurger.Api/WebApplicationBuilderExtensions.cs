// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Data;
using System.Data.Common;
using FIAP.TechChallenge.ByteMeBurger.Api.Configuration;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

internal static class WebApplicationBuilderExtensions
{
    internal static void ConfigServicesDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<DbConnectionStringBuilder>(provider =>
        {
            var mySqlOptions = provider.GetService<IOptions<MySqlSettings>>();
            return new MySqlConnectionStringBuilder()
            {
                Server = mySqlOptions!.Value.Server,
                Database = mySqlOptions.Value.Database,
                Port = mySqlOptions.Value.Port,
                Password = mySqlOptions.Value.Password,
                UserID = mySqlOptions.Value.UserId
            };
        });
        builder.Services.AddTransient<IDbConnection>(provider =>
        {
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
            var requiredService = provider.GetRequiredService<DbConnectionStringBuilder>();
            var providerFactory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
            var conn = providerFactory.CreateConnection();
            conn.ConnectionString = requiredService.ConnectionString;

            return conn;
        });
    }
}
