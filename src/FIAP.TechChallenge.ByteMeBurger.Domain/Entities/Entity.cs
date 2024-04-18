namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public abstract class Entity<T>(T id)
{
    public T Id { get; protected set; } = id;
}