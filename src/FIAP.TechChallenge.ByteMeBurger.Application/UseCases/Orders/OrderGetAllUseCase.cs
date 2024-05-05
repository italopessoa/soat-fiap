using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class OrderGetAllUseCase(IOrderRepository repository) : IOrderGetAllUseCase
{
    public async Task<ReadOnlyCollection<Order>> Execute()
    {
        var orders = await repository.GetAllAsync();
        return orders is null ? Array.Empty<Order>().AsReadOnly() : orders;
    }
}