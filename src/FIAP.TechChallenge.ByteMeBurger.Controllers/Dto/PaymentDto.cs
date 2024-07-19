namespace FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;

public class PaymentDto
{
    public Guid Id { get; set; }

    public PaymentTypeDto PaymentType { get; set; }

    public Guid OrderId { get; set; }

    public string ExternalReference { get; set; }

    public string QrCode { get; set; }

    public decimal Amount { get; set; }

    public PaymentStatusDto Status { get; set; }

    public PaymentDto()
    {

    }

    public PaymentDto(Guid paymentId, string qrCode, PaymentStatusDto status)
    {
        Id = paymentId;
        QrCode = qrCode;
        Status = status;
    }
}

