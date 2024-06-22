// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Services;

public class OrderService(
    ICreateOrderUseCase createOrderUseCase,
    IGetOrderDetailsUseCase getOrderDetailsUseCase,
    IOrderGetAllUseCase orderGetAllUseCase,
    IOrderRepository orderRepository,
    IUpdateOrderStatusUseCase updateOrderStatusUseCase)
    : IOrderService
{
    public async Task<Order> CreateAsync(string? customerCpf, List<SelectedProduct> selectedProducts)
    {
        var cpf = customerCpf is not null ? new Cpf(customerCpf) : null;
        var order = await createOrderUseCase.Execute(cpf, selectedProducts);

        await orderRepository.CreateAsync(order);

        return order;
    }

    public async Task<ReadOnlyCollection<Order>> GetAllAsync(bool listAll)
    {
        return await orderGetAllUseCase.Execute(listAll);
    }

    public async Task<Order?> GetAsync(Guid id)
    {
        return await getOrderDetailsUseCase.Execute(id);
    }

    public async Task<bool> UpdateStatusAsync(Guid orderId, OrderStatus newStatus)
    {
        return await updateOrderStatusUseCase.Execute(orderId, newStatus);
    }
}
