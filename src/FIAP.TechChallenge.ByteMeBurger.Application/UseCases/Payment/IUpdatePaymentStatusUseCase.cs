using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;

public interface IUpdatePaymentStatusUseCase
{
    Task<bool> Execute(Domain.Entities.Payment? payment, PaymentStatus status);
}
