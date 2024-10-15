using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public interface IUpdateOrderPaymentUseCase
{
    Task<bool> Execute(Guid orderId, PaymentId paymentId);
}
