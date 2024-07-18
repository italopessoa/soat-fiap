using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface IOrderService
{
    /// <summary>
    /// Create new order
    /// </summary>
    /// <param name="customerCpf">Customer cpf.</param>
    /// <param name="selectedProducts">Order items</param>
    /// <returns>Order entity</returns>
    Task<Order> CreateAsync(string? customerCpf, List<SelectedProduct> selectedProducts);

    /// <summary>
    /// Get all active orders
    /// </summary>
    /// <param name="listAll">If true it will return all orders. If false it returns only orders
    /// with status (Received, In Preparation or Ready).</param>
    /// <returns>List of orders</returns>
    Task<ReadOnlyCollection<Order>> GetAllAsync(bool listAll);

    /// <summary>
    /// Get order detail
    /// </summary>
    /// <param name="id">Order Id</param>
    /// <returns>Order entity</returns>
    Task<Order?> GetAsync(Guid id);

    /// <summary>
    /// Update Order status
    /// </summary>
    /// <param name="orderId">Order Id.</param>
    /// <param name="newStatus">New Status</param>
    /// <returns></returns>
    Task<bool> UpdateStatusAsync(Guid orderId, OrderStatus newStatus);

    /// <summary>
    /// Update Order payment
    /// </summary>
    /// <returns></returns>
    Task<bool> UpdateOrderPaymentAsync(Guid orderId, PaymentId paymentId);
}
