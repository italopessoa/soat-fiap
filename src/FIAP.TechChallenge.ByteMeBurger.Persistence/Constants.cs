// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.Persistence;

internal static class Constants
{
    internal const string GetOrderByIdQuery = @"select o.Id,
                   o.Status,
                   o.Created,
                   o.Updated,
                   o.TrackingCode,
                   o.PaymentId Id,
                   o.CustomerId as Id,
                   c.Cpf,
                   c.Name,
                   c.Email,
                   oi.ProductId,
                   oi.ProductName,
                   oi.Quantity,
                   oi.UnitPrice
                from Orders o
                         left join Customers c on o.CustomerId = c.Id
                         inner join OrderItems oi on oi.OrderId = o.Id
                where o.Id = @OrderId;";

    internal const string GetAllOrdersQuery = @"select o.Id,
                   o.Status,
                   o.Created,
                   o.Updated,
                   o.TrackingCode,
                   c.Id,
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
        "UPDATE Orders SET Status=@Status, Updated=@Updated WHERE Id = @Id";

    internal const string UpdateOrderPaymentIdQuery =
        "UPDATE Orders SET PaymentId=@PaymentId, Updated=@Updated WHERE Id = @Id";

    internal const string InsertOrderQuery =
        "insert into Orders (Id, CustomerId, Status, Created, TrackingCode) values (@Id, @CustomerId, @Status, @Created, @TrackingCode);";

    internal const string InsertOrderItemsQuery =
        "insert into OrderItems (OrderId, ProductId, ProductName, UnitPrice, Quantity) " +
        "values (@OrderId, @ProductId, @ProductName, @UnitPrice, @Quantity);";

    internal const string InsertPaymentQuery =
        "insert into Payments (Id, OrderId, Status, ExternalReference, Created, PaymentType, Amount) " +
        "values (@Id, @OrderId, @Status, @ExternalReference, @Created, @PaymentType, @Amount);";

    internal const string GetPaymentQuery = "select * from Payments where Id = @Id;";

    internal const string GetPaymentByExternalReferenceQuery = "select * from Payments where PaymentType = @PaymentType and ExternalReference = @ExternalReference;";

    internal const string UpdatePaymentStatusQuery =
        "UPDATE Payments SET Status=@Status, Updated=@Updated WHERE Id = @Id";

    internal const string InsertProductQuery =
        "insert into Products (Id, Name, Description, Category, Price, Images) values (@Id, @Name, @Description, @Category, @Price, @Images);";

    internal const string DeleteProductQuery = "delete from Products where Id = @Id;";

    internal const string UpdateProductQuery =
        "UPDATE Products SET Name=@Name, Description=@Description, Category=@Category, Price=@Price, Images=@Images WHERE Id = @Id";

    internal const string InsertCustomerQuery =
        "insert into Customers (Id, Cpf, Name, Email) values (@Id, @Cpf, @Name, @Email);";

    internal const string GetCustomerByCpfQuery = "SELECT * FROM Customers WHERE Cpf=@Cpf";
}
