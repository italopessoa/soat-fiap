// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using JetBrains.Annotations;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Test.ValueObjects;

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
