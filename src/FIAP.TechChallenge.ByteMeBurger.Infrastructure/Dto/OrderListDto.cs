using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Infrastructure.Dto;

public class OrderListDto
{
    public Guid Id { get; set; }
    public int Status { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public string? Code { get; set; }
    public Customer? Customer { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}