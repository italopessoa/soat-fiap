namespace Bmb.Orders.Domain.ValueObjects;

public record OrderTrackingCode
{
    public string Value { get; }

    public OrderTrackingCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Order tracking code cannot be null or empty.", nameof(value));
        }

        Value = value;
    }

    public static implicit operator OrderTrackingCode(string code) => new (code);
}
