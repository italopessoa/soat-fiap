// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

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
    /// <returns>List of orders</returns>
    Task<ReadOnlyCollection<Order>> GetAllAsync();

    /// <summary>
    /// Get order detail
    /// </summary>
    /// <param name="id">Order Id</param>
    /// <returns>Order entity</returns>
    Task<Order?> GetAsync(Guid id);

    /// <summary>
    /// Checkout order
    /// After checkout, the order status will be updated to "Received"
    /// </summary>
    /// <param name="id">Order Id</param>
    [Obsolete("This method will be removed in the future.")]
    Task CheckoutAsync(Guid id);
}
