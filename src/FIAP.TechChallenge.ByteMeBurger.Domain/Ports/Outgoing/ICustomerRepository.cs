using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

public interface ICustomerRepository
{
    Task<Customer?> FindByCpfAsync(string cpf);

    Task<Customer> CreateAsync(Customer customer);
}