// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class CustomerDto
{
    public Guid Id { get; set; }

    public string Cpf { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public CustomerDto()
    {
    }

    public CustomerDto(Customer customer)
    {
        Id = customer.Id;
        Cpf = customer.Cpf;
        Name = customer.Name;
        Email = customer.Email;
    }
}
