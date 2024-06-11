// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class CreateOrderUseCase : ICreateOrderUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;

    public CreateOrderUseCase(IProductRepository productRepository,
        ICustomerRepository customerRepository)
    {
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Order> Execute(Cpf? customerCpf, List<SelectedProduct> selectedProducts)
    {
        var customer = default(Customer);
        if (customerCpf is not null)
        {
            customer = await _customerRepository.FindByCpfAsync(customerCpf);
            if (customer == null)
                throw new EntityNotFoundException("Customer not found.");
        }

        var products = new Dictionary<Product, int>();
        foreach (var item in selectedProducts)
        {
            var product = await GetProduct(item.ProductId);
            if (product is null)
                throw new EntityNotFoundException($"Product '{item.ProductId}' not found.");

            products.Add(product, item.Quantity);
        }

        var order = new Order(customer,"code", products);
        DomainEventTrigger.RaiseOrderCreated(order);
        return order;
    }

    private async Task<Product?> GetProduct(Guid productId)
    {
        var product = await _productRepository.FindByIdAsync(productId);
        if (product == null)
            throw new EntityNotFoundException($"Product '{productId}' not found.");

        return product;
    }
}
