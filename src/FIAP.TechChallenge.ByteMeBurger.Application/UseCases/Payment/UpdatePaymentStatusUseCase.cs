using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;

public class UpdatePaymentStatusUseCase : IUpdatePaymentStatusUseCase
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUpdateOrderStatusUseCase _updateOrderStatusUseCase;

    public UpdatePaymentStatusUseCase(IUpdateOrderStatusUseCase updateOrderStatusUseCase,
        IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
        _updateOrderStatusUseCase = updateOrderStatusUseCase;
    }

    public async Task<bool> Execute(Domain.Entities.Payment? payment, PaymentStatus status)
    {
        if (payment is not null && payment.Status
                is not PaymentStatus.Approved
                and not PaymentStatus.Cancelled
                and not PaymentStatus.Rejected)
        {
            payment.Status = status;
            payment.Updated = DateTime.UtcNow;

            var paymentStatusUpdated = await _paymentRepository.UpdatePaymentStatusAsync(payment);

            if (paymentStatusUpdated && payment.IsApproved())
            {
                DomainEventTrigger.RaisePaymentConfirmed(payment);
                // TODO change to eventual consistency. use events to update order status
                await _updateOrderStatusUseCase.Execute(payment.OrderId, OrderStatus.Received);
            }

            return paymentStatusUpdated;
        }

        return false;
    }
}
