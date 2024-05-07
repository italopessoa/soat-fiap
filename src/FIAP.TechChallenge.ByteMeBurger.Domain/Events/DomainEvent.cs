namespace FIAP.TechChallenge.ByteMeBurger.Domain.Events;

/// <summary>
/// Base domain event
/// </summary>
/// <param name="payload"></param>
/// <typeparam name="T"></typeparam>
public abstract class DomainEvent<T>(T payload)
{
    public T Payload { get; } = payload;
}