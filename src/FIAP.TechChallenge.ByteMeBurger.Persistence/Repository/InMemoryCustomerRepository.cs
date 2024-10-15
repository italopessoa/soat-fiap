using System.Diagnostics.CodeAnalysis;
using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Repository;

[ExcludeFromCodeCoverage]
public class InMemoryCustomerRepository(IReadOnlyCollection<Customer> customersSeed) : ICustomerRepository
{
    private readonly List<Customer> _customers = customersSeed.ToList();

    public Task<Customer?> FindByCpfAsync(string cpf)
    {
        return Task.FromResult(_customers.First(c => c.Cpf == cpf) ?? default);
    }

    public Task<Customer> CreateAsync(Customer customer)
    {
        _customers.Add(customer);
        return Task.FromResult(customer);
    }

    public Task<Customer?> FindByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
