using System.Diagnostics.CodeAnalysis;
using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Repository;

[ExcludeFromCodeCoverage]
public class InMemoryPaymentRepository : IPaymentRepository
{
    private readonly List<Payment> _payments = new();

    public Task<Payment> SaveAsync(Payment payment)
    {
        _payments.Add(payment);
        return Task.FromResult(payment);
    }

    public Task<Payment?> GetPaymentAsync(PaymentId paymentId)
    {
        var payment = _payments.First(p => p.Id == paymentId) ?? default;
        return Task.FromResult(payment);
    }

    public Task<Payment?> GetPaymentAsync(string externalReference, PaymentType paymentType)
    {
        var payment = _payments.First(p => p.ExternalReference == externalReference && p.PaymentType == paymentType);
        return Task.FromResult(payment ?? default);
    }

    public Task<bool> UpdatePaymentStatusAsync(Payment payment)
    {
        var index = _payments.FindIndex(p => p.Id == payment.Id);
        if (index < 0)
        {
            return Task.FromResult(false);
        }

        _payments[index] = payment;
        return Task.FromResult(true);
    }
}
