using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public interface ICheckoutOrderUseCase
{
    Task Execute(Guid orderId);
}
