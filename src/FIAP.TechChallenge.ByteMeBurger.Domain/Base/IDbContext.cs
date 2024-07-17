// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Data;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Base;

public interface IDbContext
{
    IDbConnection CreateConnection();
}
