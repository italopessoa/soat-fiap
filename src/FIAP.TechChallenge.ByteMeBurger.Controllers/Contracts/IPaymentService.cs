using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;

public interface IPaymentService
{
    Task<PaymentDto> CreateOrderPaymentAsync(CreateOrderPaymentRequestDto command);

    Task<PaymentDto?> GetPaymentAsync(Guid paymentId);

    Task<PaymentDto?> GetPaymentAsync(string paymentId, PaymentType paymentType);

    Task<bool> SyncPaymentStatusWithGatewayAsync(string externalReference, PaymentType paymentType);
}

public record CreateOrderPaymentRequestDto(Guid OrderId, PaymentType PaymentType);
