using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class OrderGetAllUseCase(IOrderRepository repository) : IOrderGetAllUseCase
{
    public async Task<ReadOnlyCollection<Order>> Execute()
    {
        var orders = await repository.GetAllAsync();
        return orders is null
            ? Array.Empty<Order>().AsReadOnly()
            : orders
                .Where(o => o.Status == OrderStatus.Received || o.Status == OrderStatus.InPreparation ||
                            o.Status == OrderStatus.Ready)
                .OrderByDescending(o => o.Status)
                .ToList()
                .AsReadOnly();
    }
}
