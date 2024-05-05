using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;

public interface ICreateCustomerUseCase
{
    Task<Customer> Execute(Cpf cpf, string? name, string? email);
}