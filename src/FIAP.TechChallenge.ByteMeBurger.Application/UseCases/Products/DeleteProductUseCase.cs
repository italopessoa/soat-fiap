// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class DeleteProductUseCase(IProductRepository repository) : IDeleteProductUseCase
{
    public async Task<bool> Execute(Guid productId)
    {
        var deleted = await repository.DeleteAsync(productId);
        if (deleted)
            DomainEventTrigger.RaiseProductDeleted(productId);

        return deleted;
    }
}
