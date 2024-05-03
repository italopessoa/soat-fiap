using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Services;

public class OrderService(IOrderRepository repository) : IOrderService
{
    public async Task<Order> CreateAsync(Guid? customerId,
        List<(Guid productId, string productName, int quantity, decimal unitPrice)> orderItems)
    {
        var order = customerId.HasValue ? new Order(customerId.Value) : new Order();
        orderItems.ForEach(i => { order.AddOrderItem(i.productId, i.productName, i.unitPrice, i.quantity); });

        order.Checkout();
        return await repository.CreateAsync(order);
    }

    public async Task<ReadOnlyCollection<Order>> GetAllAsync()
    {
        var orders = await repository.GetAllAsync();
        return orders is null ? Array.Empty<Order>().AsReadOnly() : orders;
    }

    public async Task<Order?> GetAsync(Guid id)
    {
        return id == Guid.Empty ? null : await repository.GetAsync(id);
    }
}