using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;

public interface IUpdatePaymentStatusUseCase
{
    Task<bool> Execute(Bmb.Domain.Core.Entities.Payment? payment, PaymentStatus status);
}
