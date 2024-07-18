using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Events;

/// <summary>
/// Order status changed event
/// </summary>
/// <param name="payload"></param>
public class OrderStatusChanged((Guid OrderId, OrderStatus OldStatus, OrderStatus NewStatus) payload)
    : DomainEvent<(Guid OrderId, OrderStatus OldStatus, OrderStatus NewStatus)>(payload);
