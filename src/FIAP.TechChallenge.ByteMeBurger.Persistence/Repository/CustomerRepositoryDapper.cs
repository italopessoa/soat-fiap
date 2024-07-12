// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Data;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Persistence.Dto;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Repository;

public class CustomerRepositoryDapper(IDbConnection dbConnection, ILogger<CustomerRepositoryDapper> logger)
    : ICustomerRepository
{
    public async Task<Customer?> FindByCpfAsync(string cpf)
    {
        logger.LogInformation("Finding customer by CPF: {Cpf}", cpf);
        var customerDto = await dbConnection.QuerySingleOrDefaultAsync<CustomerDto>(
            Constants.GetCustomerByCpfQuery,
            new { Cpf = cpf });

        if (customerDto == null)
        {
            logger.LogInformation("Customer with CPF: {Cpf} not found", cpf);
        }

        return (Customer)customerDto;
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        logger.LogInformation("Creating customer with CPF: {Cpf}", customer.Cpf);
        var param = (CustomerDto)customer;
        var rowsAffected = await dbConnection.ExecuteAsync(
            Constants.InsertCustomerQuery,
            param);

        if (rowsAffected > 0)
        {
            logger.LogInformation("Customer with CPF: {Cpf} created", customer.Cpf);
        }
        else
        {
            logger.LogError("Failed to create customer with CPF: {Cpf}", customer.Cpf);
        }

        return customer;
    }
}
