// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.Persistence;

public static class Constants
{
    internal const string GetOrderByIdQuery = @"select o.Id,
                   o.Status,
                   o.Created,
                   o.Updated,
                   o.TrackingCode,
                   o.CustomerId as Id,
                   c.Cpf,
                   c.Name,
                   c.Email,
                   p.Id,
                   p.OrderId,
                   oi.ProductId,
                   oi.ProductName,
                   oi.Quantity,
                   oi.UnitPrice
                from Orders o
                         left join Customers c on o.CustomerId = c.Id
                         left join Payments p on p.OrderId = o.Id
                         inner join OrderItems oi on oi.OrderId = o.Id
                where o.Id = @OrderId;";

    internal const string GetAllOrdersQuery = @"select o.Id,
                   o.Status,
                   o.Created,
                   o.Updated,
                   o.TrackingCode,
                   o.Id,
                   c.Cpf,
                   c.Name,
                   c.Email,
                   p.Id,
                   p.OrderId,
                   oi.ProductId,
                   oi.ProductName,
                   oi.Quantity,
                   oi.UnitPrice
            from Orders o
                     inner join OrderItems oi on oi.OrderId = o.Id
                     left join Customers c on c.Id = o.CustomerId
                     left join Payments p on p.OrderId = o.Id;";

    internal const string UpdateOrderStatusQuery =
        "UPDATE Orders SET Status=@Status, Updated=@LastUpdate WHERE Id = @Id";

    internal const string InsertOrderQuery =
        "insert into Orders (Id, CustomerId, Status, Created, TrackingCode) values (@Id, @CustomerId, @Status, @Created, @TrackingCode);";

    internal const string InsertOrderItemsQuery =
        "insert into OrderItems (OrderId, ProductId, ProductName, UnitPrice, Quantity) " +
        "values (@OrderId, @ProductId, @ProductName, @UnitPrice, @Quantity);";

    public const string InsertPaymentQuery =
        "insert into Payments (Id, OrderId, Status, Created, PaymentType, Amount) " +
        "values (@Id, @OrderId, @Status, @Created, @PaymentType, @Amount);";
}
