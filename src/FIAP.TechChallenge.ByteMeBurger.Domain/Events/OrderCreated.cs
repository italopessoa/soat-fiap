using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Events;

/// <summary>
/// Order created (checkout) event
/// </summary>
/// <param name="payload"></param>
public class OrderCreated(Order payload) : DomainEvent<Order>(payload);