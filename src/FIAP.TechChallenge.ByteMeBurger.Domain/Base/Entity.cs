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

    public TId Id { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is Entity<TId> otherObject)
        {
            return !EqualityComparer<TId>.Default.Equals(Id, default(TId)) && Id!.Equals(otherObject.Id);
        }

        return false;
    }

    public DateTime Created { get; set; }

    public DateTime? Updated { get; set; }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
