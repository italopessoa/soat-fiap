namespace FIAP.TechChallenge.ByteMeBurger.Domain.Events;

/// <summary>
/// Product deleted event
/// </summary>
/// <param name="payload"></param>
public class ProductDeleted(Guid payload) : DomainEvent<Guid>(payload);