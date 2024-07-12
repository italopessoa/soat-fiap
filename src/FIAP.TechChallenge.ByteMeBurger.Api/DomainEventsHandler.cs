// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using Microsoft.Extensions.Caching.Hybrid;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

/// <summary>
/// Simple class to handle domain events
/// </summary>
[ExcludeFromCodeCoverage]
public class DomainEventsHandler : IDisposable
{
    private readonly ILogger<DomainEventsHandler> _logger;
    private readonly HybridCache _cache;

    public DomainEventsHandler(ILogger<DomainEventsHandler> logger, HybridCache cache)
    {
        _logger = logger;
        _cache = cache;
        DomainEventTrigger.ProductCreated += OnProductCreated;
        DomainEventTrigger.ProductDeleted += OnProductDeleted;
        DomainEventTrigger.ProductUpdated += OnProductUpdated;
        DomainEventTrigger.OrderCreated += OnOrderCreated;
        DomainEventTrigger.OrderPaymentConfirmed += OnOrderPaymentConfirmed;
        DomainEventTrigger.OrderStatusChanged += OnOrderStatusChanged;
        DomainEventTrigger.CustomerRegistered += OnCustomerRegistered;
        DomainEventTrigger.PaymentCreated += OnPaymentCreated;
    }

    private void OnCustomerRegistered(object? sender, CustomerRegistered e)
    {
        _logger.LogInformation("New Customer registered: {@Customer}", e.Payload);
        _logger.LogInformation("Sending email to customer: {CustomerName}", e.Payload.Name);
    }

    private void OnOrderStatusChanged(object? sender, OrderStatusChanged e)
    {
        InvalidateOrderCache(e.Payload.orderId);
        _logger.LogInformation("Order: {Id} status changed from '{oldStatus}' to '{newStatus}'", e.Payload.orderId,
            e.Payload.oldStatus, e.Payload.newStatus);
    }

    private void OnOrderPaymentConfirmed(object? sender, OrderPaymentConfirmed e)
    {
        _logger.LogInformation("Order: {Id} payment confirmed", e.Payload);
    }

    private void OnOrderCreated(object? sender, OrderCreated e)
    {
        InvalidateOrderCache(e.Payload.Id);
        _logger.LogInformation("New Order created: {Id}", e.Payload.Id);
    }

    private void OnProductUpdated(object? sender, ProductUpdated e)
    {
        _logger.LogInformation("Product: {@oldProduct} updated {@newProduct}", e.Payload.newProduct,
            e.Payload.newProduct);
    }

    private void OnProductDeleted(object? sender, ProductDeleted e)
    {
        _logger.LogInformation("Product deleted: {Id}", e.Payload);
    }

    private void OnProductCreated(object? sender, ProductCreated e)
    {
        _logger.LogInformation("Product created: {@Product}", e.Payload);
    }

    private void OnPaymentCreated(object? sender, PaymentCreated e)
    {
        _logger.LogInformation("Payment {PaymentId} created for Order: {OrderId}", e.Payload.Id.Code,
            e.Payload.Id.OrderId);
    }

    private void InvalidateOrderCache(Guid orderId)
    {
        _cache.RemoveAsync($"order-{orderId}").ConfigureAwait(false);
        InvalidateOrderList();
    }

    private void InvalidateOrderList()
    {
        _cache.RemoveAsync("orders-filter").ConfigureAwait(false);
        _cache.RemoveAsync($"orders-nonFilter").ConfigureAwait(false);
    }

    public void Dispose()
    {
        DomainEventTrigger.ProductCreated -= OnProductCreated;
        DomainEventTrigger.ProductDeleted -= OnProductDeleted;
        DomainEventTrigger.ProductUpdated -= OnProductUpdated;
        DomainEventTrigger.OrderCreated -= OnOrderCreated;
        DomainEventTrigger.OrderPaymentConfirmed -= OnOrderPaymentConfirmed;
        DomainEventTrigger.OrderStatusChanged -= OnOrderStatusChanged;
        DomainEventTrigger.CustomerRegistered -= OnCustomerRegistered;
        DomainEventTrigger.PaymentCreated -= OnPaymentCreated;
    }
}
