// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> FindByCpfAsync(string cpf);

    Task<Customer> CreateAsync(Customer customer);
}
