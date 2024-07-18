using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface IOrderTrackingCodeService
{
    Task<OrderTrackingCode> GetNextAsync();

    OrderTrackingCode GetNext();
}
