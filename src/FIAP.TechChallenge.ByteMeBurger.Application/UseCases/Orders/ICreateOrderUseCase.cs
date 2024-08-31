using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public interface ICreateOrderUseCase
{
    Task<Order> Execute(Customer? customer, List<SelectedProduct> selectedProducts);
}
