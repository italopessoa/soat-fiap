using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public class Payment : Entity<PaymentId>, IAggregateRoot
{
    public string Type { get; set; }

    public PaymentType PaymentType { get; set; }

    public Guid OrderId { get; set; }

    public string ExternalReference { get; set; }

    public string QrCode { get; set; }

    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; }

    public Payment()
    {
        Created = DateTime.UtcNow;
    }

    public Payment(PaymentId id, string qrCode, decimal amount, PaymentType paymentType = PaymentType.Test)
        : base(id)
    {
        Id = id;
        Status = PaymentStatus.Pending;
        QrCode = qrCode;
        Amount = amount;
        Created = DateTime.UtcNow;
        PaymentType = paymentType;
    }

    public bool IsApproved() => Status == PaymentStatus.Approved;
}
