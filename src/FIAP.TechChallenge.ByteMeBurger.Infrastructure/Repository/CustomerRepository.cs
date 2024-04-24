using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

namespace FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;

public class CustomerRepository: ICustomerRepository
{
    public Task<Customer?> FindByCpfAsync(string cpf)
    {
        throw new NotImplementedException();
    }

    public Task<Customer> CreateAsync(Customer customer)
    {
        throw new NotImplementedException();
    }
}