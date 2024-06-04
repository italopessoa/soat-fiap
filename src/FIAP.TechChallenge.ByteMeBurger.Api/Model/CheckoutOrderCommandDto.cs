// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.ComponentModel.DataAnnotations;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;


/// <summary>
/// Command to checkout an order.
/// </summary>
/// <param name="Id">Order Id.</param>
public record CheckoutOrderCommandDto([Required] Guid Id);
