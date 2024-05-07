using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Events;

/// <summary>
/// Product created event
/// </summary>
/// <param name="payload"></param>
public class ProductCreated(Product payload) : DomainEvent<Product>(payload);