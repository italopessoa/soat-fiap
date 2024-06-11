// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.DomainServices;

public class OrderTrackingCodeService : IOrderTrackingCodeService
{
    public Task<OrderTrackingCode> GetNextAsync()
    {
        return Task.FromResult(GetNext());
    }

    public OrderTrackingCode GetNext()
    {
        var currentTime = DateTime.UtcNow;
        const string lettersOnly = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        const string reversedCharacters = "87654321ZYXVUTSRQPMLKJIHFEDCB";

        var hour = currentTime.Hour;
        var minute = currentTime.Minute;
        var second = currentTime.Second;
        var millisecond = currentTime.Millisecond;

        var partA = GetLetter(hour, lettersOnly);
        var partB = millisecond % 2 == 0 ? string.Empty : GetLetter(minute, reversedCharacters);
        var partC = GetLetter(second, reversedCharacters);

        var key = Guid.NewGuid().ToString()
            .Split("-")[2]
            .Substring(1, 3);

        return new OrderTrackingCode($"{partA}{partB}{partC}-{key}".ToUpper());
    }

    private static string GetLetter(int number, string alphabet)
    {
        var adjustedNumber = number % alphabet.Length;
        var letterIndex = adjustedNumber > 0 ? adjustedNumber - 1 : adjustedNumber;

        return alphabet.Substring(letterIndex, 1);
    }
}
