using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers;

internal static class Mapper
{
    internal static Customer ToDomain(this CustomerDto? customer)
    {
        return customer is null ? null : new Customer(customer.Id, customer.Cpf, customer.Name!, customer.Email!);
    }
}
