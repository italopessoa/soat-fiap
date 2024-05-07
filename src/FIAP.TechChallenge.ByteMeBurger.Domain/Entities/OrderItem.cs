using FIAP.TechChallenge.ByteMeBurger.Domain.Base;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public class OrderItem : Entity<Guid>
{
    public OrderItem()
    {
    }

    public OrderItem(Guid orderId, Guid productId, string productName, decimal unitPrice, int quantity)
        : base(Guid.NewGuid())
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(unitPrice);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        
        if (Guid.Empty == orderId)
            throw new ArgumentException("Invalid OrderId", nameof(orderId));
        if (Guid.Empty == productId)
            throw new ArgumentException("Invalid ProductId", nameof(productId));

        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Guid OrderId { get; private set; }

    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; } = string.Empty;

    public decimal UnitPrice { get; private set; }

    public int Quantity { get; private set; }
}