using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Events;

/// <summary>
/// Customer registered event
/// </summary>
/// <param name="payload"></param>
public class CustomerRegistered(Customer payload) : DomainEvent<Customer>(payload);
