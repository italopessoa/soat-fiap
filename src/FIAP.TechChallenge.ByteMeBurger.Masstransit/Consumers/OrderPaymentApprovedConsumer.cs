using Bmb.Domain.Core.Events.Notifications;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.ByteMeBurger.Masstransit.Consumers;

public class OrderPaymentApprovedConsumer(
    ILogger<OrderPaymentApprovedConsumer> logger,
    IUseCase<UpdateOrderStatusRequest, bool> updateOrderStatusUseCase) : IConsumer<OrderPaymentConfirmed>
{
    public async Task Consume(ConsumeContext<OrderPaymentConfirmed> context)
    {
        try
        {
            logger.LogInformation(
                "OrderPaymentApprovedConsumer: Received OrderPaymentConfirmed event for OrderId: {OrderId}",
                context.Message.OrderId);
            await updateOrderStatusUseCase.ExecuteAsync(new UpdateOrderStatusRequest(context.Message.OrderId,
                OrderStatus.Received));
            logger.LogInformation(
                "OrderPaymentApprovedConsumer: Successfully updated order status for OrderId: {OrderId}",
                context.Message.OrderId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "OrderPaymentApprovedConsumer: Error processing OrderPaymentConfirmed event for OrderId: {OrderId}",
                context.Message.OrderId);
            throw;
        }
    }
}
