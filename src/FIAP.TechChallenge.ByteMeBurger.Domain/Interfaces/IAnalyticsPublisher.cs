using FIAP.TechChallenge.ByteMeBurger.Domain.Events;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface IAnalyticsPublisher
{
    Task PublishAsync<T>(DomainEvent<T> @event);
}
