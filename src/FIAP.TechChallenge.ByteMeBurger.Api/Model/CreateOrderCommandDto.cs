// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class CreateOrderCommandDto
{
    public string? Cpf { get; set; } = null;
    public List<CreateOrderItemDto> Items { get; set; } = new();
}
