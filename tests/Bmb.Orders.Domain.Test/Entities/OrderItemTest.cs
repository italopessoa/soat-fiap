using Bmb.Domain.Core.Entities;
using Bmb.Orders.Domain.Entities;
using FluentAssertions;
using JetBrains.Annotations;

namespace Bmb.Orders.Domain.Test.Entities;

[TestSubject(typeof(OrderItem))]
public class OrderItemTest
{
    [Fact]
    public void OrderItem_InvalidProductId_ThrowsError()
    {
        // Arrange
        // Act
        var func = () => new OrderItem(Guid.NewGuid(), Guid.Empty,"product name", 10,1);

        // Assert
        func.Should().Throw<ArgumentException>().And.Message.Should()
            .Be("Invalid ProductId (Parameter 'productId')");
    }

    [Fact]
    public void OrderItem_InvalidOrderId_ThrowsError()
    {
        // Arrange
        // Act
        var func = () => new OrderItem(Guid.Empty, Guid.NewGuid(),"product name", 10,1);

        // Assert
        func.Should().Throw<ArgumentException>().And.Message.Should()
            .Be("Invalid OrderId (Parameter 'orderId')");
    }

    [Fact]
    public void OrderItem_InvalidProductName_ThrowsError()
    {
        // Arrange
        // Act
        var func = () => new OrderItem(Guid.NewGuid(), Guid.NewGuid(),"", 10,1);

        // Assert
        func.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void OrderItem_InvalidUnitPrice_ThrowsError()
    {
        // Arrange
        // Act
        var func = () => new OrderItem(Guid.NewGuid(), Guid.NewGuid(),"product name", 0,1);

        // Assert
        func.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void OrderItem_InvalidQuantity_ThrowsError()
    {
        // Arrange
        // Act
        var func = () => new OrderItem(Guid.NewGuid(), Guid.NewGuid(),"product name", 10,-1);

        // Assert
        func.Should().Throw<ArgumentOutOfRangeException>();
    }
}
