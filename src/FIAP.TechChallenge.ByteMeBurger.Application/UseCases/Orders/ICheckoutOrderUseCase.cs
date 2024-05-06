using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public interface ICheckoutOrderUseCase
{
    Task<Order> Execute(Cpf? customerCpf, List<(Guid productId, int quantity)> orderItems);
}