using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;

public interface IOrderService
{
    /// <summary>
    /// Create new order
    /// </summary>
    /// <param name="customerCpf">Customer cpf.</param>
    /// <param name="orderItems">Order items</param>
    /// <returns>Order entity</returns>
    Task<Order> CreateAsync(Cpf? customerCpf, List<(Guid productId, int quantity)> orderItems);
    
    /// <summary>
    /// Get all active orders
    /// </summary>
    /// <returns>List of orders</returns>
    Task<ReadOnlyCollection<Order>> GetAllAsync();
    
    /// <summary>
    /// Get order detail
    /// </summary>
    /// <param name="id">Order Id</param>
    /// <returns>Order entity</returns>
    Task<Order?> GetAsync(Guid id);
}