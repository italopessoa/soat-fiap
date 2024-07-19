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

        return customerDto.FromDtoToEntity();
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        logger.LogInformation("Creating customer with CPF: {Cpf}", customer.Cpf);
        var rowsAffected = await dbConnection.ExecuteAsync(
            Constants.InsertCustomerQuery,
            customer.FromEntityToDto());

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
