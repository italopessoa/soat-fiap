using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using Bmb.Domain.Core.Events;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;

public class UpdatePaymentStatusUseCase : IUpdatePaymentStatusUseCase
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUseCase<UpdateOrderStatusRequest, bool> _updateOrderStatusUseCase;

    public UpdatePaymentStatusUseCase(IUseCase<UpdateOrderStatusRequest, bool> updateOrderStatusUseCase,
        IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
        _updateOrderStatusUseCase = updateOrderStatusUseCase;
    }

    public async Task<bool> Execute(Bmb.Domain.Core.Entities.Payment? payment, PaymentStatus status)
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
                // DomainEventTrigger.RaisePaymentConfirmed(payment);
                await _updateOrderStatusUseCase.ExecuteAsync(new UpdateOrderStatusRequest(payment.OrderId, OrderStatus.Received));
            }

            return paymentStatusUpdated;
        }

        return false;
    }
}
