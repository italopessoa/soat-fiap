using FIAP.TechChallenge.ByteMeBurger.Domain.Base;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public class OrderItem : Entity<Guid>
{
    public OrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
        : base(Guid.NewGuid())
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}