using System.Collections.ObjectModel;
using Bmb.Orders.Domain.Entities;

namespace Bmb.Orders.Domain.Contracts;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);

    Task<ReadOnlyCollection<Order>> GetAllAsync();

    Task<Order?> GetAsync(Guid orderId);

    Task<bool> UpdateOrderStatusAsync(Order order);

    Task<bool> UpdateOrderPaymentAsync(Order order);
}
