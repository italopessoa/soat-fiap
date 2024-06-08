// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);

    Task<ReadOnlyCollection<Order>> GetAllAsync();

    Task<Order?> GetAsync(Guid orderId);

    Task<bool> UpdateOrderStatusAsync(Order order);
}
