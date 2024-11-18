using Bmb.Domain.Core.Events.Notifications;
using Bmb.Domain.Core.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.ByteMeBurger.Masstransit.Consumers;

public class OrderStatusChangedConsumer(
    ILogger<OrderStatusChangedConsumer> logger,
    IUseCase<UpdateOrderStatusRequest, bool> updateOrderStatusUseCase)
    : BaseConsumer<OrderStatusChanged, UpdateOrderStatusRequest, bool>(logger, updateOrderStatusUseCase)
{
    protected override UpdateOrderStatusRequest PrepareRequest(OrderStatusChanged request)
    {
        return new UpdateOrderStatusRequest(request.OrderId, request.Status);
    }
}
