namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public abstract class Entity<TId>(TId id)
{
    public TId Id { get; protected set; } = id;
    
    public override bool Equals(object? obj)
    {
        if (obj is Entity<TId> otherObject)
        {
            return Id != null && Id.Equals(otherObject.Id);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}