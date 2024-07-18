using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface IPaymentService
{
    Task<Payment> CreateOrderPaymentAsync(CreateOrderPaymentRequestDto command);

    Task<Payment?> GetPaymentAsync(PaymentId paymentId);

    Task<Payment?> GetPaymentAsync(string paymentId, PaymentType paymentType);

    Task<bool> SyncPaymentStatusWithGatewayAsync(string externalReference, PaymentType paymentType);
}

public record CreateOrderPaymentRequestDto(Guid OrderId, PaymentType PaymentType);
