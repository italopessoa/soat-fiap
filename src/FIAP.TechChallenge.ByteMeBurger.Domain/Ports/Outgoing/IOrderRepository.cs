using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
    
    Task<ReadOnlyCollection<Order>> GetAllAsync();
    
    Task<Order?> GetAsync(Guid orderId);
}