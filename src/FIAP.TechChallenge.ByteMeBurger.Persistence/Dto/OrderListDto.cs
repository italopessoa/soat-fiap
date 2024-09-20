namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Dto;

public class OrderListDto
{
    public Guid Id { get; set; }
    public int Status { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public string TrackingCode { get; set; } = string.Empty;
    public Guid? PaymentId { get; set; }
}
