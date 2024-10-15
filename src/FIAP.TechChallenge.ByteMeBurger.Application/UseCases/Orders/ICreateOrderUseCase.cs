using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public interface ICreateOrderUseCase
{
    Task<Order> Execute(Customer? customer, List<SelectedProduct> selectedProducts);
}
