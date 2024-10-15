using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public interface IUpdateOrderStatusUseCase
{
    Task<bool> Execute(Guid orderId, OrderStatus newStatus);
}
