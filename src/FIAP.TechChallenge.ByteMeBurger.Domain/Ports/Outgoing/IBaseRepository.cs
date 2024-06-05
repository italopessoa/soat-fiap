// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.Application.Ports.Outgoing;

[Obsolete("This interface is not being used and will be removed in the future.")]
public interface IBaseRepository<in TEntityId, T> where T : new()
{
    Task<IReadOnlyCollection<T>> GetAll();

    Task<T> FindById(TEntityId entityId);

    Task<T> Update(T entity);

    Task Create(T entity);

    Task Delete(TEntityId entityId);
}
