using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;

public interface IOrderService
{
    Task<Order> CreateAsync(string customerId, List<(Guid productId, string productName, int quantity, decimal unitPrice)> orderItems);
    
    Task<ReadOnlyCollection<Order>> GetAllAsync();
    
    Task<Order?> GetAsync(Guid id);
}