// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Events;

/// <summary>
/// Base domain event
/// </summary>
/// <param name="payload"></param>
/// <typeparam name="T"></typeparam>
public abstract class DomainEvent<T>(T payload)
{
    public T Payload { get; } = payload;
}
