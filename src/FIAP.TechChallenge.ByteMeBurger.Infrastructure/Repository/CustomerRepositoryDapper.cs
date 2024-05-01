using System.Data;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

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
        return await _dbConnection.QuerySingleOrDefaultAsync<Customer>("SELECT * FROM Customer WHERE Id=@Id",
            new { Id = cpf });
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        var rowsAffected = await _dbConnection.ExecuteAsync(
            "insert into Customer (Id, Name, Email) values (@Id, @Name, @Email);",
            customer);

        return customer;
    }
}