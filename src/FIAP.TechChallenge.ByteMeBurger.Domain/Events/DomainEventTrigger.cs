// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

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
    public static event EventHandler<PaymentCreated>? PaymentCreated;

    internal static void RaiseProductCreated(ProductCreated e)
    {
        ProductCreated?.Invoke(null, e);
    }

    internal static void RaiseOrderCreated(Order order)
    {
        OrderCreated?.Invoke(null, new OrderCreated(order));
    }

    internal static void RaiseProductDeleted(Guid productId)
    {
        ProductDeleted?.Invoke(null, new ProductDeleted(productId));
    }

    internal static void RaiseProductUpdated(ProductUpdated e)
    {
        ProductUpdated?.Invoke(null, e);
    }

    internal static void RaiseOrderStatusChanged(Guid orderId, OrderStatus oldStatus, OrderStatus newStatus)
    {
        OrderStatusChanged?.Invoke(null, new OrderStatusChanged((orderId, oldStatus, newStatus)));
    }

    internal static void RaisePaymentConfirmed(Payment payment)
    {
        OrderPaymentConfirmed?.Invoke(null, new OrderPaymentConfirmed(payment));
    }

    internal static void RaiseCustomerRegistered(CustomerRegistered e)
    {
        CustomerRegistered?.Invoke(null, e);
    }

    internal static void RaisePaymentCreated(PaymentCreated e)
    {
        PaymentCreated?.Invoke(null, e);
    }
}
