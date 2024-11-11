using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using Bmb.Domain.Core.Base;
using Bmb.Domain.Core.Events;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;

public class CreatePaymentUseCase : ICreatePaymentUseCase
{
    private readonly IPaymentGatewayFactoryMethod _paymentGatewayFactory;
    private readonly IGetOrderDetailsUseCase _getOrderDetailsUseCase;

    public CreatePaymentUseCase(IPaymentGatewayFactoryMethod paymentGatewayFactory, IGetOrderDetailsUseCase getOrderDetailsUseCase)
    {
        _paymentGatewayFactory = paymentGatewayFactory;
        _getOrderDetailsUseCase = getOrderDetailsUseCase;
    }

    public async Task<Bmb.Domain.Core.Entities.Payment?> Execute(Guid orderId, PaymentType paymentType)
    {
        var order = await _getOrderDetailsUseCase.Execute(orderId);

        if (order is null)
            throw new EntityNotFoundException("Order not found.");

        if (order.PaymentId is not null)
            throw new DomainException("There's already a Payment for the order.");

        var paymentGateway = _paymentGatewayFactory.Create(paymentType);
        var payment = await paymentGateway.CreatePaymentAsync(order);
        // if (payment != null)
        // {
        //     DomainEventTrigger.RaisePaymentCreated(new PaymentCreated(payment));
        // }
        return payment;
    }
}
