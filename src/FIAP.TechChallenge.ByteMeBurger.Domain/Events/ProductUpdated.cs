using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Events;

/// <summary>
/// Product updated event
/// </summary>
/// <param name="payload"></param>
public class ProductUpdated((Product oldProduct, Product newProduct) payload)
    : DomainEvent<(Product oldProduct, Product newProduct)>(payload);