using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> FindByCpfAsync(string cpf);

    Task<Customer> CreateAsync(Customer customer);
}
