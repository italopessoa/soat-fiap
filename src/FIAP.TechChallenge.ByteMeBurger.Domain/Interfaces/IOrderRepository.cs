using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);

    Task<ReadOnlyCollection<Order>> GetAllAsync();

    Task<Order?> GetAsync(Guid orderId);

    Task<bool> UpdateOrderStatusAsync(Order order);

    Task<bool> UpdateOrderPaymentAsync(Order order);
}
