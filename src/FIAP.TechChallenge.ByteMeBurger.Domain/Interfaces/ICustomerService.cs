// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface ICustomerService
{
    /// <summary>
    /// Find customer by cpf
    /// </summary>
    /// <param name="cpf">Customer's cpf</param>
    /// <returns>Customer entity</returns>
    Task<Customer?> FindByCpfAsync(string cpf);

    /// <summary>
    /// Create new customer
    /// </summary>
    /// <param name="cpf">Cpf</param>
    /// <param name="name">Name</param>
    /// <param name="email">Email</param>
    /// <returns>Customer entity</returns>
    Task<Customer> CreateAsync(string cpf, string? name, string? email);
}
