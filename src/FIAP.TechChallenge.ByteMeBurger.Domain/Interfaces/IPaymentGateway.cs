using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface IPaymentGateway
{
    Task<Payment> CreatePaymentAsync(Order order);

    Task<PaymentStatus?> GetPaymentStatusAsync(string paymentId);
}
