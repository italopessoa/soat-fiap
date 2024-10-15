using System.Collections.ObjectModel;
using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class OrderGetAllUseCase(IOrderRepository repository) : IOrderGetAllUseCase
{
    public async Task<ReadOnlyCollection<Order>> Execute(bool listAll)
    {
        var orders = (await repository.GetAllAsync() ?? Enumerable.Empty<Order>());
        return (listAll
                ? orders
                : orders.Where(o => o.Status is OrderStatus.Received or OrderStatus.InPreparation or OrderStatus.Ready)
            ).OrderByDescending(o => o.Status)
            .ThenBy(o => o.Created)
            .ToList()
            .AsReadOnly();
    }
}
