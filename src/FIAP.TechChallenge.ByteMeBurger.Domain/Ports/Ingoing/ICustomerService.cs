using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;

public interface ICustomerService
{
    Task<Customer?> FindByCpfAsync(string cpf);

    Task<Customer> CreateAsync(string cpf, string? name, string? email);
}