using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> SaveAsync(Payment payment);

    Task<Payment?> GetPaymentAsync(PaymentId paymentId);

    Task<Payment?> GetPaymentAsync(string externalReference, PaymentType paymentType);

    Task<bool> UpdatePaymentStatusAsync(Payment payment);
}
