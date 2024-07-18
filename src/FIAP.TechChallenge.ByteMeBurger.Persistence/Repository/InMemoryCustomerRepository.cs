using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Repository;

[ExcludeFromCodeCoverage]
public class InMemoryCustomerRepository(IReadOnlyCollection<Customer> customersSeed) : ICustomerRepository
{
    private readonly List<Customer> _customers = customersSeed.ToList();

    public Task<Customer?> FindByCpfAsync(string cpf)
    {
        return Task.FromResult(_customers.FirstOrDefault(c => c.Cpf == cpf));
    }

    public Task<Customer> CreateAsync(Customer customer)
    {
        _customers.Add(customer);
        return Task.FromResult(customer);
    }
}
