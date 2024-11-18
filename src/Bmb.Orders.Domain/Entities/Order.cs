using Bmb.Domain.Core.Base;
using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.ValueObjects;
using OrderTrackingCode = Bmb.Orders.Domain.ValueObjects.OrderTrackingCode;

namespace Bmb.Orders.Domain.Entities;

public class Order : Entity<Guid>, IAggregateRoot
{
    private List<OrderItem> _orderItems = Array.Empty<OrderItem>().ToList();

    public Bmb.Domain.Core.Entities.Customer? Customer { get; private set; }

    public OrderTrackingCode TrackingCode { get; set; }

    public OrderStatus Status { get; private set; } = OrderStatus.PaymentPending;

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public decimal Total => _orderItems.Sum(o => o.UnitPrice * o.Quantity);

    public PaymentId? PaymentId { get; set; }

    public Order()
        : base(Guid.NewGuid())
    {
        Created = DateTime.UtcNow;
    }

    internal Order(Guid customerId)
        : this(Guid.NewGuid(), new Bmb.Domain.Core.Entities.Customer(customerId))
    {
        Created = DateTime.UtcNow;
    }

    internal Order(Bmb.Domain.Core.Entities.Customer customer)
        : base(Guid.NewGuid())
    {
        Customer = customer;
        Created = DateTime.UtcNow;
    }

    internal Order(Guid id, Bmb.Domain.Core.Entities.Customer customer)
        : base(id)
    {
        Customer = customer;
        Created = DateTime.UtcNow;
    }

    public Order(Guid id, Bmb.Domain.Core.Entities.Customer? customer, OrderStatus status, OrderTrackingCode trackingCode, DateTime created,
        DateTime? updated)
        : base(id)
    {
        Customer = customer;
        Status = status;
        TrackingCode = trackingCode;
        Created = created;
        Updated = updated;
    }

    public Order(Bmb.Domain.Core.Entities.Customer? customer, OrderTrackingCode trackingCode, Dictionary<Product, int> selectedProducts)
        : base(Guid.NewGuid())
    {
        Customer = customer;
        TrackingCode = trackingCode;
        Created = DateTime.UtcNow;

        if (selectedProducts.Count == 0)
        {
            throw new DomainException("An Order must have at least one item");
        }

        foreach (var (product, quantity) in selectedProducts)
        {
            AddOrderItem(product.Id, product.Name, product.Price, quantity);
        }
    }

    public void AddOrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        if (Status == OrderStatus.PaymentPending)
            _orderItems.Add(new OrderItem(Id, productId, productName, unitPrice, quantity));
        else
            throw new DomainException(
                $"Cannot add items to an Order with status {Status}. Items can only be added when the status is PaymentPending.");
    }

    public void LoadItems(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        _orderItems.Add(new OrderItem(Id, productId, productName, unitPrice, quantity));
    }

    public void SetPayment(PaymentId paymentId)
    {
        if (PaymentId is not null)
        {
            throw new DomainException("Order already has a payment intent.");
        }

        PaymentId = paymentId;
        Update();
    }

    public void SetCustomer(Bmb.Domain.Core.Entities.Customer customer)
    {
        Customer = customer;
    }

    public void ConfirmPayment()
    {
        if (Status != OrderStatus.PaymentPending)
            throw new DomainException($"Payment cannot be confirmed because of order status '{Status}'.");

        Status = OrderStatus.Received;
        Update();
    }

    public void InitiatePrepare()
    {
        if (Status != OrderStatus.Received)
            throw new DomainException("Cannot start preparing if order isn't received.");

        Status = OrderStatus.InPreparation;
        Update();
    }

    public void FinishPreparing()
    {
        if (Status != OrderStatus.InPreparation)
            throw new DomainException("Cannot Finish preparing order if it's not In Preparation yet.");

        Status = OrderStatus.Ready;
        Update();
    }

    public void DeliverOrder()
    {
        if (Status != OrderStatus.Ready)
            throw new DomainException("Cannot Deliver order if it's not Ready yet.");

        Status = OrderStatus.Completed;
        Update();
    }

    public void SetTrackingCode(OrderTrackingCode code)
    {
        if (Status != OrderStatus.PaymentPending)
            throw new DomainException("Cannot set status code for an existing Order.");

        TrackingCode = code;
    }

    private void Update() => Updated = DateTime.UtcNow;
};
