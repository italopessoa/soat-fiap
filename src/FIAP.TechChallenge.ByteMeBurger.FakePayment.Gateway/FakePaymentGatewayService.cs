using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.FakePayment.Gateway;

[ExcludeFromCodeCoverage]
public class FakePaymentGatewayService : IPaymentGateway
{
    public Task<Payment?> CreatePaymentAsync(Order order)
    {
        var id = Guid.NewGuid();
        var payment = new Payment
        {
            Id = new PaymentId(id),
            OrderId = order.Id,
            PaymentType = PaymentType.Test,
            Amount = order.Total,
            Created = DateTime.Now,
            Status = PaymentStatus.Pending,
            QrCode = "https://fake.qrcode.com",
            ExternalReference = id.ToString()
        };
        return Task.FromResult(payment);
    }

    public Task<PaymentStatus?> GetPaymentStatusAsync(string paymentId)
    {
        PaymentStatus? status = PaymentStatus.Approved;
        return Task.FromResult(status);
    }
}
