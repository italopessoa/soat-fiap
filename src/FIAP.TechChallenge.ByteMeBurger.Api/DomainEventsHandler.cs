using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
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
    private readonly IOrderService _orderService;

    public DomainEventsHandler(ILogger<DomainEventsHandler> logger, HybridCache cache, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _cache = cache;
        using (var scope = serviceProvider.CreateScope())
        {
            _orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        }

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
        _logger.LogInformation("New {EventName} event from '{OldStatus}' to '{NewStatus}': OrderId {OrderId}",
            nameof(OrderStatusChanged),
            e.Payload.OldStatus, e.Payload.NewStatus, e.Payload.OrderId);
    }

    private async void OnOrderPaymentConfirmed(object? sender, OrderPaymentConfirmed e)
    {
        _logger.LogInformation("Order: {OrderId} payment confirmed", e.Payload);
        InvalidateOrderCache(e.Payload.OrderId);
        try
        {
            await _orderService.UpdateStatusAsync(e.Payload.OrderId, OrderStatus.Received);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error updating order status {OrderId}", e.Payload.OrderId);
            throw;
        }
    }

    private void OnOrderCreated(object? sender, OrderCreated e)
    {
        InvalidateOrderCache(e.Payload.Id);
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
    }

    private async void OnPaymentCreated(object? sender, PaymentCreated e)
    {
        _logger.LogInformation("Payment {PaymentId} created for Order: {OrderId}", e.Payload.OrderId,
            e.Payload.Id.Value);

        try
        {
            await _orderService.UpdateOrderPaymentAsync(e.Payload.OrderId, e.Payload.Id);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error updating order payment {OrderId}", e.Payload.OrderId);
            throw;
        }
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
