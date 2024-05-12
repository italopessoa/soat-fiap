using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Events;

/// <summary>
/// Order status changed event
/// </summary>
/// <param name="payload"></param>
public class OrderStatusChanged((Guid orderId, OrderStatus old, OrderStatus newStatus) payload)
    : DomainEvent<(Guid orderId, OrderStatus oldStatus, OrderStatus newStatus)>(payload);
