using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;

public interface ICreatePaymentUseCase
{
    Task<Domain.Entities.Payment?> Execute(Guid orderId, PaymentType paymentType);
}
