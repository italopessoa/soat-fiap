// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Customers;

/// <summary>
/// Customer detail
/// </summary>
public class CustomerViewModel
{

    public Guid Id { get; set; }

    /// <summary>
    /// Customer CPF
    /// </summary>
    public string Cpf { get; set; }

    /// <summary>
    /// Customer name
    /// </summary>
    public string? Name { get; set; }


    /// <summary>
    /// Customer E-mail
    /// </summary>
    public string? Email { get; set; }
}
