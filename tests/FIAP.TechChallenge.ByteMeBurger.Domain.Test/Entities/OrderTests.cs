// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using AutoFixture;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Test.Entities;

public class OrderTests
{
    private static readonly Cpf CustomerCpf = "946.571.740-10";

    [Fact]
    public void Order_NewOrder_HasId()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var order = new Order(customerId);

        // Assert
        using (new AssertionScope())
        {
            order.Id.Should().NotBe(Guid.Empty);
            order.Status.Should().Be(OrderStatus.PaymentPending);
            order.Customer.Should().NotBeNull();
            order.Customer!.Id.Should().Be(customerId);
        }
    }

    [Fact]
    public void Order_NoCustomer_HasId()
    {
        // Arrange & Act
        var order = new Order();

        // Assert
        using (new AssertionScope())
        {
            order.Id.Should().NotBe(Guid.Empty);
            order.Status.Should().Be(OrderStatus.PaymentPending);
            order.Customer.Should().BeNull();
        }
    }

    [Fact]
    public void Order_CheckoutOrder_UpdateStatus()
    {
        // Arrange & Act
        var customerId = Guid.NewGuid();
        var order = new Order(customerId);
        order.AddOrderItem(Guid.NewGuid(), "bread", 1, 5);
        order.AddOrderItem(Guid.NewGuid(), "milk shake", 2, 6);

        // Assert
        using (new AssertionScope())
        {
            order.Id.Should().NotBe(Guid.Empty);
            order.Status.Should().Be(OrderStatus.PaymentPending);
            order.Created.Should().NotBe(default);
            order.Customer.Should().NotBeNull();
        }
    }

    [Fact]
    public void Order_ConfirmPayment_AlreadyConfirmed_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddOrderItem(Guid.NewGuid(), "bread", 2, 5);
        order.AddOrderItem(Guid.NewGuid(), "milk shake", 3, 4);
        order.ConfirmPayment();

        // Act
        var func = () => order.ConfirmPayment();

        // Assert
        func.Should().ThrowExactly<DomainException>().And.Message.Should()
            .Be($"Payment cannot be confirmed because of order status '{OrderStatus.Received}'.");
    }

    [Fact]
    public void Order_FinishPreparing_NotInitiated_ThrowsError()
    {
        // Arrange
        // Act
        var func = () => new Order().FinishPreparing();

        // Assert
        func.Should().ThrowExactly<DomainException>().And.Message.Should()
            .Be("Cannot Finish preparing order if it's not In Preparation yet.");
    }

    [Fact]
    public void Order_Deliver_NotFinished_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddOrderItem(Guid.NewGuid(), "bread", 2.5m, 4);
        order.AddOrderItem(Guid.NewGuid(), "milk shake", 3, 4);
        order.SetTrackingCode(new OrderTrackingCode("trackingCode"));
        order.ConfirmPayment();

        // Act
        var func = () => order.DeliverOrder();

        // Assert
        using (new AssertionScope())
        {
            func.Should().ThrowExactly<DomainException>().And.Message.Should()
                .Be("Cannot Deliver order if it's not Ready yet.");
            order.TrackingCode.Should().NotBeNull();
        }
    }

    [Fact]
    public void Order_ValidOrder()
    {
        // Arrange
        var initDate = DateTime.UtcNow;
        var customerId = Guid.NewGuid();

        var order = new Order(customerId);
        order.SetTrackingCode(new OrderTrackingCode("trackingCode"));
        order.AddOrderItem(Guid.NewGuid(), "bread", 10, 1);
        order.AddOrderItem(Guid.NewGuid(), "milk shake", 6, 2);

        // Act
        order.ConfirmPayment();
        var receivedDate = order.Updated;
        order.InitiatePrepare();
        var preparingDate = order.Updated;
        order.FinishPreparing();
        var doneDate = order.Updated;
        order.DeliverOrder();
        var finishedDate = order.Updated;

        // Assert
        using (new AssertionScope())
        {
            order.Customer.Should().NotBeNull();
            order.Customer!.Id.Should().Be(customerId);
            order.Created.Should().BeAfter(initDate);
            receivedDate.Should().BeAfter(order.Created);
            order.Created.Should().BeBefore(preparingDate.Value);
            doneDate.Should().BeAfter(preparingDate.Value);
            finishedDate.Should().BeAfter(doneDate.Value);
            order.Status.Should().Be(OrderStatus.Completed);
            order.Total.Should().Be(22);
            order.TrackingCode.Should().NotBeNull();
        }
    }

    [Fact]
    public void Order_SetPayment_PaymentAlreadyExists_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddOrderItem(Guid.NewGuid(), "bread", 2, 5);
        order.SetPayment(new Fixture().Create<PaymentId>());

        // Act
        var func = () => order.SetPayment(new Fixture().Create<PaymentId>());;

        // Assert
        func.Should().ThrowExactly<DomainException>().And.Message.Should()
            .Be("Order already has a payment intent.");
    }
}
