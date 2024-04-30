using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public class Order : Entity<Guid>
{
    private List<OrderItem> _orderItems;
    public Customer Customer { get; private set; }

    public string? TrackingCode { get; private set; }

    public OrderStatus Status { get; private set; }

    public DateTime CreationDate { get; private set; }

    public DateTime LastUpdate { get; private set; }

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public decimal Total => _orderItems.Sum(o => o.UnitPrice * o.Quantity);


    public Order()
        : this(Guid.NewGuid().ToString())
    {
    }

    public Order(string customerId)
        : base(Guid.NewGuid())
    {
        _orderItems = Enumerable.Empty<OrderItem>().ToList();
        Customer = new Customer(customerId);
    }

    public void AddOrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        if (Status == OrderStatus.PaymentPending)
            _orderItems.Add(new OrderItem(productId, productName, unitPrice, quantity));
        else
            throw new InvalidOperationException($"Cannot add items to an Order if it's {Status}");
    }

    public void ValidateCheckout()
    {
        if (!_orderItems.Any())
        {
            throw new InvalidOperationException("An Order must have at least one item");
        }
    }

    public void Checkout()
    {
        ValidateCheckout();
        CreationDate = DateTime.UtcNow;
    }

    public void ConfirmPayment()
    {
        if (CreationDate == default)
            throw new InvalidOperationException("Cannot confirm");

        Status = OrderStatus.Received;
        TrackingCode = GenerateCode(CreationDate);
    }

    public void InitiatePrepare()
    {
        if (Status != OrderStatus.Received)
            throw new InvalidOperationException("Cannot start preparing if order isn't confirmed.");

        Status = OrderStatus.Preparing;
        Update();
    }

    public void FinishPreparing()
    {
        if (Status != OrderStatus.Preparing)
            throw new InvalidOperationException("Cannot Finish order if it's not Preparing yet.");

        Status = OrderStatus.Done;
        Update();
    }

    public void DeliverOrder()
    {
        if (Status != OrderStatus.Done)
            throw new InvalidOperationException("Cannot Deliver order if it's not Finished yet.");

        Status = OrderStatus.Finished;
        Update();
    }

    private void Update() => LastUpdate = DateTime.UtcNow;

    private static string GetLetter(int number, string alphabet)
    {
        var adjustedNumber = number % alphabet.Length;
        var letterIndex = adjustedNumber > 0 ? adjustedNumber - 1 : adjustedNumber;

        return alphabet.Substring(letterIndex, 1);
    }

    private string GenerateCode(DateTime confirmationDate)
    {
        const string lettersOnly = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        const string reversedCharacters = "87654321ZYXVUTSRQPMLKJIHFEDCB";

        var hour = confirmationDate.Hour;
        var minute = confirmationDate.Minute;
        var second = confirmationDate.Second;
        var millisecond = confirmationDate.Millisecond;

        var partA = GetLetter(hour, lettersOnly);
        var partB = millisecond % 2 == 0 ? string.Empty : GetLetter(minute, reversedCharacters);
        var partC = GetLetter(second, reversedCharacters);

        var key = Guid.NewGuid().ToString()
            .Split("-")[2]
            .Substring(1, 3);

        return $"{partA}{partB}{partC}-{key}".ToUpper();
    }
};