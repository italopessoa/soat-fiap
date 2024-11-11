using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public interface IUpdateOrderStatusUseCase
{
    Task<bool> Execute(Guid orderId, OrderStatus newStatus);
}

public interface IUseCase<in TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request);
}

public record UpdateOrderStatusRequest(Guid OrderId, OrderStatus NewStatus);
