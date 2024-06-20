// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public class Order : Entity<Guid>, IAggregateRoot
{
    private List<OrderItem> _orderItems = Array.Empty<OrderItem>().ToList();

    public Customer? Customer { get; private set; }

    public OrderTrackingCode TrackingCode { get; private set; }

    public OrderStatus Status { get; private set; } = OrderStatus.Received;

    public DateTime Created { get; private set; }

    public DateTime? LastUpdate { get; private set; }

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public decimal Total => _orderItems.Sum(o => o.UnitPrice * o.Quantity);

    public PaymentId? PaymentId { get; set; }

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

    public Order(Guid id, Customer? customer, OrderStatus status, OrderTrackingCode trackingCode, DateTime created,
        DateTime? updated)
        : base(id)
    {
        Customer = customer;
        Status = status;
        TrackingCode = trackingCode;
        Created = created;
        LastUpdate = updated;
    }

    public Order(Customer? customer, OrderTrackingCode trackingCode, Dictionary<Product, int> selectedProducts)
        : base(Guid.NewGuid())
    {
        Customer = customer;
        TrackingCode = trackingCode;
        Created = DateTime.UtcNow;
        Status = OrderStatus.Received;

        if (!selectedProducts.Any())
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
        if (Status == OrderStatus.Received)
            _orderItems.Add(new OrderItem(Id, productId, productName, unitPrice, quantity));
        else
            throw new DomainException(
                $"Cannot add items to an Order with status {Status}. Items can only be added when the status is PaymentPending.");
    }

    public void LoadItems(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        _orderItems.Add(new OrderItem(Id, productId, productName, unitPrice, quantity));
    }

    public void Create()
    {
        if (!_orderItems.Any())
        {
            throw new DomainException("An Order must have at least one item");
        }

        Created = DateTime.UtcNow;
        Status = OrderStatus.Received;
    }

    public void ConfirmPayment()
    {
        if (Status != OrderStatus.Received)
            throw new DomainException($"Payment cannot be confirmed because of order status '{Status}'.");

        Status = OrderStatus.InPreparation;
        Update();
    }

    public void FinishPreparing()
    {
        if (Status != OrderStatus.InPreparation)
            throw new DomainException("Cannot Finish order if it's not In Preparation yet.");

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
        if (Status != OrderStatus.Received)
            throw new DomainException("Cannot set status code for a existing Order.");

        TrackingCode = code;
    }

    private void Update() => LastUpdate = DateTime.UtcNow;
};
