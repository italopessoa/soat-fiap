using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;

public interface ICreatePaymentUseCase
{
    Task<Bmb.Domain.Core.Entities.Payment?> Execute(Guid orderId, PaymentType paymentType);
}
