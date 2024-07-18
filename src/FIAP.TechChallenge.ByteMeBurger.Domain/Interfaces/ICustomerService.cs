using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface ICustomerService
{
    /// <summary>
    /// Find customer by cpf
    /// </summary>
    /// <param name="cpf">Customer's cpf</param>
    /// <returns>Customer entity</returns>
    Task<Customer?> FindByCpfAsync(string cpf);

    /// <summary>
    /// Create new customer
    /// </summary>
    /// <param name="cpf">Cpf</param>
    /// <param name="name">Name</param>
    /// <param name="email">Email</param>
    /// <returns>Customer entity</returns>
    Task<Customer> CreateAsync(string cpf, string? name, string? email);
}
