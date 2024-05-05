using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public class Order : Entity<Guid>
{
    private List<OrderItem> _orderItems = Enumerable.Empty<OrderItem>().ToList();

    public Customer? Customer { get; private set; }

    public string? TrackingCode { get; private set; }

    public OrderStatus Status { get; private set; }

    public DateTime Created { get; private set; }

    public DateTime? LastUpdate { get; private set; }

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public decimal Total => _orderItems.Sum(o => o.UnitPrice * o.Quantity);


    public Order()
        : base(Guid.NewGuid())
    {
    }

    public Order(Guid customerId)
        : this(Guid.NewGuid(), new Customer(customerId))
    {
    }

    public Order(Customer customer)
        : base(Guid.NewGuid())
    {
        Customer = customer;
    }

    public Order(Guid id, Customer customer)
        : base(id)
    {
        Customer = customer;
    }

    public Order(Guid id, Customer customer, OrderStatus status, string? trackingCode, DateTime created, DateTime? updated)
        : base(id)
    {
        Customer = customer;
        Status = status;
        TrackingCode = trackingCode;
        Created = created;
        LastUpdate = updated;
    }

    public void AddOrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        if (Status == OrderStatus.PaymentPending)
            _orderItems.Add(new OrderItem(Id, productId, productName, unitPrice, quantity));
        else
            throw new DomainException($"Cannot add items to an Order if it's {Status}");
    }

    public void LoadItems(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        _orderItems.Add(new OrderItem(Id, productId, productName, unitPrice, quantity));
    }

    public void ValidateCheckout()
    {
        if (!_orderItems.Any())
        {
            throw new DomainException("An Order must have at least one item");
        }
    }

    public void Checkout()
    {
        ValidateCheckout();
        Created = DateTime.UtcNow;
    }

    public void ConfirmPayment()
    {
        if (Created == default)
            throw new DomainException("Cannot confirm");

        Status = OrderStatus.Received;
        TrackingCode = GenerateCode(Created);
    }

    public void InitiatePrepare()
    {
        if (Status != OrderStatus.Received)
            throw new DomainException("Cannot start preparing if order isn't confirmed.");

        Status = OrderStatus.Preparing;
        Update();
    }

    public void FinishPreparing()
    {
        if (Status != OrderStatus.Preparing)
            throw new DomainException("Cannot Finish order if it's not Preparing yet.");

        Status = OrderStatus.Done;
        Update();
    }

    public void DeliverOrder()
    {
        if (Status != OrderStatus.Done)
            throw new DomainException("Cannot Deliver order if it's not Finished yet.");

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