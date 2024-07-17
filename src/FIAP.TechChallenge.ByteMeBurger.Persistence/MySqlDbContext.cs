// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Data;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence;

public class MySqlDbContext : IDbContext, IDisposable
{
    private readonly IConfiguration _configuration;
    private IDbConnection? _dbConnection;

    public MySqlDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        if (_dbConnection is null)
        {
            var connection = _configuration.GetConnectionString("MySql");
            _dbConnection = new MySqlConnection(connection);
            _dbConnection.Open();
        }

        return _dbConnection;
    }

    public void Dispose()
    {
        _dbConnection?.Close();
    }
}
