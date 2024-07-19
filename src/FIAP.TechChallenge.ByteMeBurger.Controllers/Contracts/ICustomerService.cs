using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;

public interface ICustomerService
{
    /// <summary>
    /// Find customer by cpf
    /// </summary>
    /// <param name="cpf">Customer's cpf</param>
    /// <returns>Customer entity</returns>
    Task<CustomerDto?> FindByCpfAsync(string cpf);

    /// <summary>
    /// Create new customer
    /// </summary>
    /// <param name="cpf">Cpf</param>
    /// <param name="name">Name</param>
    /// <param name="email">Email</param>
    /// <returns>Customer entity</returns>
    Task<CustomerDto> CreateAsync(string cpf, string? name, string? email);
}
