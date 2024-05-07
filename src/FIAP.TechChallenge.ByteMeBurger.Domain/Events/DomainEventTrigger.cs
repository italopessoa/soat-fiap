namespace FIAP.TechChallenge.ByteMeBurger.Domain.Events;

public static class DomainEventTrigger
{
    public static event EventHandler<ProductCreated>? ProductCreated;
    public static event EventHandler<ProductDeleted>? ProductDeleted;
    public static event EventHandler<ProductUpdated>? ProductUpdated;
    public static event EventHandler<OrderCreated>? OrderCreated;
    public static event EventHandler<OrderPaymentConfirmed>? OrderPaymentConfirmed;
    public static event EventHandler<OrderStatusChanged>? OrderStatusChanged;
    public static event EventHandler<CustomerRegistered>? CustomerRegistered;
    
    internal static void RaiseProductCreated(ProductCreated e)
    {
        ProductCreated?.Invoke(null, e);
    }

    internal static void RaiseOrderCreated(OrderCreated e)
    {
        OrderCreated?.Invoke(null, e);
    }

    internal static void RaiseProductDeleted(ProductDeleted e)
    {
        ProductDeleted?.Invoke(null, e);
    }

    internal static void RaiseProductUpdated(ProductUpdated e)
    {
        ProductUpdated?.Invoke(null, e);
    }

    internal static void RaiseOrderStatusChanged(OrderStatusChanged e)
    {
        OrderStatusChanged?.Invoke(null, e);
    }

    internal static void RaiseOrderPaymentConfirmed(OrderPaymentConfirmed e)
    {
        OrderPaymentConfirmed?.Invoke(null, e);
    }

    internal static void RaiseCustomerRegistered(CustomerRegistered e)
    {
        CustomerRegistered?.Invoke(null, e);
    }
}