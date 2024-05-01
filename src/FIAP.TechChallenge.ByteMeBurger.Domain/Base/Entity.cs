namespace FIAP.TechChallenge.ByteMeBurger.Domain.Base;

public abstract class Entity<TId>
{
    protected Entity()
    {
        
    }

    protected Entity(TId id)
    {
        Id = id;
    }
    
    public TId Id { get; protected set; }

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