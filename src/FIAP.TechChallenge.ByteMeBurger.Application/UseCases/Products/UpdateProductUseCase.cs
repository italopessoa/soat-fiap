// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class UpdateProductUseCase(IProductRepository repository) : IUpdateProductUseCase
{
    public async Task<bool> Execute(Guid id, string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images)
    {
        var currentProduct = await repository.FindByIdAsync(id);
        if (currentProduct is null)
        {
            return false;
        }

        currentProduct.Update(new Product(id, name, description, category, price, images));
        if (await repository.UpdateAsync(currentProduct))
        {
            DomainEventTrigger.RaiseProductUpdated(new ProductUpdated((currentProduct, currentProduct)));
            return true;
        }

        return false;
    }
}
