// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.ComponentModel.DataAnnotations;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public record CreateCustomerCommand
{
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MaxLength(14)]
    public string Cpf { get; set; }
}
