// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Dto;

public class OrderListDto
{
    public Guid Id { get; set; }
    public int Status { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public string TrackingCode { get; set; }
    public Customer? Customer { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
