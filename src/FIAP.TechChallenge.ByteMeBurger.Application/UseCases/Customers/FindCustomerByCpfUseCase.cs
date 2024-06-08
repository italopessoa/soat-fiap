// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;

public class FindCustomerByCpfUseCase(ICustomerRepository customerRepository) : IFindCustomerByCpfUseCase
{
    public async Task<Customer?> Execute(string cpf)
    {
        return await customerRepository.FindByCpfAsync(cpf);
    }
}
