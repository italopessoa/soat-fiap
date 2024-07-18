using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Events;

/// <summary>
/// Payment created event
/// </summary>
/// <param name="payload"></param>
public class PaymentCreated (Payment payload): DomainEvent<Payment>(payload);
