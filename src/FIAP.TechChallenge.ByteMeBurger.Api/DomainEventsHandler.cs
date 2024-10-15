using System.Diagnostics.CodeAnalysis;
using Bmb.Domain.Core.Events;
using Bmb.Domain.Core.Interfaces;
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
    private readonly IAnalyticsPublisher _publisher;

    /// <summary>
    /// Simple event handler class
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="cache">Cache</param>
    /// <param name="publisher">Events publisher</param>
    public DomainEventsHandler(ILogger<DomainEventsHandler> logger, HybridCache cache, IAnalyticsPublisher publisher)
    {
        _logger = logger;
        _cache = cache;
        _publisher = publisher;

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
        _publisher.PublishAsync(e).ConfigureAwait(false);
    }

    private void OnOrderStatusChanged(object? sender, OrderStatusChanged e)
    {
        _logger.LogInformation("New {EventName} event from '{OldStatus}' to '{NewStatus}': OrderId {OrderId}",
            nameof(OrderStatusChanged),
            e.Payload.OldStatus, e.Payload.NewStatus, e.Payload.OrderId);
        InvalidateOrderCache(e.Payload.OrderId);
    }

    private void OnOrderPaymentConfirmed(object? sender, OrderPaymentConfirmed e)
    {
        _logger.LogInformation("Order: {OrderId} payment confirmed", e.Payload);
        InvalidateOrderCache(e.Payload.OrderId);
    }

    private void OnOrderCreated(object? sender, OrderCreated e)
    {
        InvalidateOrderList();
        _publisher.PublishAsync(e).ConfigureAwait(false);
        _logger.LogInformation("New {EventName} event: OrderId {OrderId}", nameof(OrderCreated), e.Payload.Id);
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
        _publisher.PublishAsync(e).ConfigureAwait(false);
    }

    private void OnPaymentCreated(object? sender, PaymentCreated e)
    {
        _logger.LogInformation("Payment {PaymentId} created for Order: {OrderId}", e.Payload.Id.Value,
            e.Payload.OrderId);
    }

    private void InvalidateOrderCache(Guid orderId)
    {
        _cache.RemoveAsync($"order-{orderId}").ConfigureAwait(false);
        InvalidateOrderList();
    }

    private void InvalidateOrderList()
    {
        _cache.RemoveAsync("orders-filter").ConfigureAwait(false);
        _cache.RemoveAsync("orders-nonFilter").ConfigureAwait(false);
    }

    /// <summary>
    /// Dispose
    /// </summary>
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
        GC.SuppressFinalize(this);
    }
}
