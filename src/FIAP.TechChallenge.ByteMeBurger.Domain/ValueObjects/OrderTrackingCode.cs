// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

public record OrderTrackingCode
{
    public string Value { get; }

    public OrderTrackingCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Order tracking code cannot be null or empty.", nameof(value));
        }

        Value = value;
    }

    public static implicit operator OrderTrackingCode(string code) => new (code);
}
