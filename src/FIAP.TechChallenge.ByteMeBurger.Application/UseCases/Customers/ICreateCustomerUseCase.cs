using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;

public interface ICreateCustomerUseCase
{
    Task<Customer> Execute(Cpf cpf, string? name, string? email);
}
