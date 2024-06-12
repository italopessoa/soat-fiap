// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

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
        // Arrange
        // Act
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
    public void Order_CreateEmptyOrder_ThrowsError()
    {
        // Arrange
        var order = new Order();

        // Act
        var func = () => order.Create();

        // Assert
        func.Should().ThrowExactly<DomainException>();
    }

    [Fact]
    public void Order_CheckoutOrder_UpdateStatus()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var order = new Order(customerId);
        order.AddOrderItem(Guid.NewGuid(), "bread", 1, 5);
        order.AddOrderItem(Guid.NewGuid(), "milk shake", 2, 6);

        // Act
        order.Create();

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
    public void Order_ConfirmPayment_NotPending_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddOrderItem(Guid.NewGuid(), "bread", 2, 5);
        order.AddOrderItem(Guid.NewGuid(), "milk shake", 3, 4);
        order.Create();
        order.ConfirmPayment();
        // Act
        var func = () => order.ConfirmPayment();

        // Assert
        func.Should().ThrowExactly<DomainException>().And.Message.Should()
            .Be("Payment cannot be confirmed if order isn't pending.");
    }

    [Fact]
    public void Order_ConfirmReceiving_NoPayment_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddOrderItem(Guid.NewGuid(), "bread", 2, 5);
        order.AddOrderItem(Guid.NewGuid(), "milk shake", 3, 4);
        order.Create();
        // Act
        var func = () => order.ConfirmReceiving();

        // Assert
        func.Should().ThrowExactly<DomainException>().And.Message.Should()
            .Be("Cannot confirm receiving if payment isn't confirmed.");
    }

    [Fact]
    public void Order_Initiate_NotConfirmed_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddOrderItem(Guid.NewGuid(), "bread", 2, 5);
        order.AddOrderItem(Guid.NewGuid(), "milk shake", 3, 4);

        // Act
        var func = () => order.InitiatePrepare();

        // Assert
        func.Should().ThrowExactly<DomainException>().And.Message.Should()
            .Be("Cannot start preparing if order isn't received.");
    }

    [Fact]
    public void Order_Finish_NotInitiated_ThrowsError()
    {
        // Arrange
        // Act
        var func = () => new Order().FinishPreparing();

        // Assert
        func.Should().ThrowExactly<DomainException>().And.Message.Should()
            .Be("Cannot Finish order if it's not In Preparation yet.");
    }

    [Fact]
    public void Order_Deliver_NotFinished_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddOrderItem(Guid.NewGuid(), "bread", 2.5m, 4);
        order.AddOrderItem(Guid.NewGuid(), "milk shake", 3, 4);
        order.SetTrackingCode(new OrderTrackingCode("trackingCode"));
        order.Create();
        order.ConfirmPayment();
        order.ConfirmReceiving();
        order.InitiatePrepare();

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
        order.Create();
        order.ConfirmPayment();
        order.ConfirmReceiving();
        order.InitiatePrepare();
        var preparingDate = order.LastUpdate;
        order.FinishPreparing();
        var doneDate = order.LastUpdate;
        order.DeliverOrder();
        var finishedDate = order.LastUpdate;

        // Assert
        using (new AssertionScope())
        {
            order.Customer.Should().NotBeNull();
            order.Customer!.Id.Should().Be(customerId);
            order.Created.Should().BeAfter(initDate);
            order.Created.Should().BeBefore(preparingDate.Value);
            doneDate.Should().BeAfter(preparingDate.Value);
            finishedDate.Should().BeAfter(doneDate.Value);
            order.Status.Should().Be(OrderStatus.Completed);
            order.Total.Should().Be(22);
            order.TrackingCode.Should().NotBeNull();
        }
    }
}
