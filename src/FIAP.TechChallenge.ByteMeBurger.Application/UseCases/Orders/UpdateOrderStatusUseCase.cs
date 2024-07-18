using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class UpdateOrderStatusUseCase(IOrderRepository repository) : IUpdateOrderStatusUseCase
{
    public async Task<bool> Execute(Guid orderId, OrderStatus newStatus)
    {
        var order = await repository.GetAsync(orderId);
        if (order == null)
        {
            throw new EntityNotFoundException($"Order '{orderId}' not found.");
        }

        if (order.Status == OrderStatus.Completed)
        {
            throw new DomainException("Order is already completed.");
        }

        var oldStatus = order.Status;
        Action updateStatus = newStatus switch
        {
            OrderStatus.Received => order.ConfirmPayment,
            OrderStatus.InPreparation => order.InitiatePrepare,
            OrderStatus.Ready => order.FinishPreparing,
            OrderStatus.Completed => order.DeliverOrder,
            _ => throw new DomainException(nameof(newStatus), new Exception($"Invalid Status '{newStatus}'"))
        };

        updateStatus.Invoke();

        var updated = await repository.UpdateOrderStatusAsync(order);
        if (updated)
        {
            DomainEventTrigger.RaiseOrderStatusChanged(order.Id, oldStatus, order.Status);
        }

        return updated;
    }
}
