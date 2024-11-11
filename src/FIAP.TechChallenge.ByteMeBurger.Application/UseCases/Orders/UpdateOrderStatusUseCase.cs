using Bmb.Domain.Core.Base;
using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class UpdateOrderStatusUseCase(IOrderRepository repository) : IUseCase<UpdateOrderStatusRequest, bool>
{
    public async Task<bool> ExecuteAsync(UpdateOrderStatusRequest request)
    {
        var order = await TryValidateRequest(request);

        Action updateStatus = request.NewStatus switch
        {
            OrderStatus.Received => order.ConfirmPayment,
            OrderStatus.InPreparation => order.InitiatePrepare,
            OrderStatus.Ready => order.FinishPreparing,
            OrderStatus.Completed => order.DeliverOrder,
            _ => throw new DomainException(nameof(request.NewStatus),
                new Exception($"Invalid Status '{request.NewStatus}'"))
        };

        updateStatus.Invoke();

        return await repository.UpdateOrderStatusAsync(order);
    }

    private async Task<Order> TryValidateRequest(UpdateOrderStatusRequest request)
    {
        var order = await repository.GetAsync(request.OrderId);
        if (order == null)
        {
            throw new EntityNotFoundException($"Order '{request.OrderId}' not found.");
        }

        if (order.Status == OrderStatus.Completed)
        {
            throw new DomainException("Order is already completed.");
        }

        return order;
    }
}
