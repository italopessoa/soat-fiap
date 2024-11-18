using Bmb.Orders.Domain.ValueObjects;

namespace Bmb.Orders.Domain.Contracts;

public interface IOrderTrackingCodeService
{
    Task<OrderTrackingCode> GetNextAsync();

    OrderTrackingCode GetNext();
}
