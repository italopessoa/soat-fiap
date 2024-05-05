using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;

public interface IFindCustomerByCpfUseCase
{
    Task<Customer?> Execute(string cpf);
}