namespace FIAP.TechChallenge.ByteMeBurger.Application.Ports.Outgoing;

[Obsolete("This interface is not being used and will be removed in the future.")]
public interface IBaseRepository<in TEntityId, T> where T : new()
{
    Task<IReadOnlyCollection<T>> GetAll();

    Task<T> FindById(TEntityId entityId);

    Task<T> Update(T entity);

    Task Create(T entity);

    Task Delete(TEntityId entityId);
}