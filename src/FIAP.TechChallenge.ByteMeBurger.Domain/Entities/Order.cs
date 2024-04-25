using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public class Order : Entity<Guid>
{
    private List<Product> _products;

    public Customer Customer { get; private set; }

    public string? TrackingCode { get; private set; }

    public OrderStatus Status { get; private set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastUpdate { get; set; }

    public IReadOnlyList<Product> Products => _products.AsReadOnly();

    public decimal Total => _products.Sum(p => p.Price);


    public Order()
        : base(Guid.NewGuid())
    {
        _products = Enumerable.Empty<Product>().ToList();
        Customer = new Customer(Guid.NewGuid().ToString());
    }

    public Order(string customerId)
        : base(Guid.NewGuid())
    {
        _products = Enumerable.Empty<Product>().ToList();
        Customer = new Customer(customerId);
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public void Validate()
    {
        if (!Products.Any())
        {
            throw new InvalidOperationException("An Order must have at least one item");
        }
    }

    public void Checkout()
    {
        Validate();
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

    private bool IsFullCombo() => _products
        .Select(p => p.Category)
        .Distinct()
        .Count() == 4;

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

        return $"{partA}{partB}{partC}-{(IsFullCombo() ? "#" : "")}{key}".ToUpper();
    }
};