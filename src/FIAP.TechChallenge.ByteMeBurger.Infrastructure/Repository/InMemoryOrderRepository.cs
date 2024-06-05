// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

namespace FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;

[ExcludeFromCodeCoverage]
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();

    public Task<Order> CreateAsync(Order order)
    {
        _orders.Add(order);
        return Task.FromResult(order);
    }

    public Task<ReadOnlyCollection<Order>> GetAllAsync() => Task.FromResult(_orders.AsReadOnly());

    public Task<Order?> GetAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
            return Task.FromResult<Order?>(null);

        var orderIndex = _orders.FindIndex(o => o.Id == orderId);
        return orderIndex >= 0 ? Task.FromResult(_orders[orderIndex]) : Task.FromResult<Order?>(null);
    }

    public Task<bool> UpdateOrderStatusAsync(Order order)
    {
        throw new NotImplementedException();
    }
}
