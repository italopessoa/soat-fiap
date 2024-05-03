using System.Data;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Infrastructure.Dto;

namespace FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;

public class CustomerRepositoryDapper : ICustomerRepository
{
    private readonly IDbConnection _dbConnection;

    public CustomerRepositoryDapper(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Customer?> FindByCpfAsync(string cpf)
    {
        var customerDto = await _dbConnection.QuerySingleOrDefaultAsync<CustomerDto>(
            "SELECT * FROM Customers WHERE Cpf=@Cpf",
            new { Cpf = cpf });

        return (Customer)customerDto;
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        var param = (CustomerDto)customer;
        var rowsAffected = await _dbConnection.ExecuteAsync(
            "insert into Customers (Id, Cpf, Name, Email) values (@Id, @Cpf, @Name, @Email);",
            param);

        return customer;
    }
}