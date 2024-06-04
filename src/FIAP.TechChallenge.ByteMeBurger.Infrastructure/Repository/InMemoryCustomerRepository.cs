// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

namespace FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;

[ExcludeFromCodeCoverage]
public class InMemoryCustomerRepository(IReadOnlyCollection<Customer> customersSeed) : ICustomerRepository
{
    private readonly List<Customer> _customers = customersSeed.ToList();

    public Task<Customer?> FindByCpfAsync(string cpf)
    {
        return Task.FromResult(_customers.FirstOrDefault(c => c.Cpf == cpf));
    }

    public Task<Customer> CreateAsync(Customer customer)
    {
        _customers.Add(customer);
        return Task.FromResult(customer);
    }
}
