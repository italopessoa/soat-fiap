using Bmb.Domain.Core.Events.Notifications;
using Bmb.Domain.Core.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using MassTransit;

namespace FIAP.TechChallenge.ByteMeBurger.Masstransit.Consumers;

public class OrderPaymentCreatedConsumer(IUpdateOrderPaymentUseCase updateOrderPaymentUseCase) : IConsumer<PaymentCreated>
{
    public async Task Consume(ConsumeContext<PaymentCreated> context)
    {
        await updateOrderPaymentUseCase.Execute(context.Message.OrderId, new PaymentId(context.Message.PaymentId));
    }
}
