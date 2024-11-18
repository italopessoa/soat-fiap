using Bmb.Orders.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Bmb.Orders.Domain.Test.ValueObjects;

[TestSubject(typeof(OrderTrackingCode))]
public class OrderTrackingCodeTest
{
    [Fact]
    public void OrderTrackingCode_CreationWithNonEmptyValue_ShouldSucceed()
    {
        // Arrange
        var trackingCodeValue = "TRACK1234";

        // Act
        var trackingCode = new OrderTrackingCode(trackingCodeValue);

        // Assert
        Assert.Equal(trackingCodeValue, trackingCode.Value);
    }

    [Fact]
    public void OrderTrackingCode_CreationWithNullOrEmptyValue_ShouldThrowArgumentException()
    {
        // Arrange
        string trackingCodeValue = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new OrderTrackingCode(trackingCodeValue));

        trackingCodeValue = string.Empty;
        Assert.Throws<ArgumentException>(() => new OrderTrackingCode(trackingCodeValue));
    }
}
