using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Infrastructure.Dto;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;

public class OrderRepositoryDapper(IDbConnection dbConnection, ILogger<OrderRepositoryDapper> logger)
    : IOrderRepository
{
    public async Task<Order> CreateAsync(Order order)
    {
        logger.LogInformation("Creating order with ID: {OrderId}", order.Id);
        dbConnection.Open();
        var transaction = dbConnection.BeginTransaction();
        {
            try
            {
                await dbConnection.ExecuteAsync(
                    "insert into Orders (Id, CustomerId, Status, Created) values (@Id, @CustomerId, @Status, @Created);",
                    new
                    {
                        Id = order.Id,
                        CustomerId = order.Customer.Id,
                        Status = (int)order.Status,
                        Created = order.Created
                    });
                await dbConnection.ExecuteAsync(
                    "insert into OrderItems (OrderId, ProductId, ProductName, UnitPrice, Quantity) " +
                    "values (@OrderId, @ProductId, @ProductName, @UnitPrice, @Quantity);",
                    order.OrderItems);

                transaction.Commit();
                logger.LogInformation("Order with ID: {OrderId} created", order.Id);
                return order;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error creating order with ID: {OrderId}", order.Id);
                transaction.Rollback();
                throw;
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }

    public async Task<ReadOnlyCollection<Order>> GetAllAsync()
    {
        logger.LogInformation("Getting all orders");
        var ordersDictionary = new Dictionary<Guid, Order>();

        await dbConnection.QueryAsync<OrderListDto, CustomerDto, Order>(
            @"select o.Id,
                   o.Status,
                   o.Created,
                   o.Updated,
                   o.Code,
                   oi.OrderId,
                   oi.ProductId,
                   oi.ProductName,
                   oi.Quantity,
                   oi.UnitPrice,
                   c.id    as Id,
                   c.cpf   as Cpf,
                   c.name  as Name,
                   c.email as Email
            from Orders o
                     inner join OrderItems oi on oi.OrderId = o.Id
                     left join Customers c on c.Id = o.CustomerId
            where o.Status in (2, 3, 4)
            order by o.Status desc, o.Created;",
            (orderListDto, customerDto) =>
            {
                if (ordersDictionary.TryGetValue(orderListDto.Id, out var order))
                {
                    order.LoadItems(orderListDto.ProductId, orderListDto.ProductName, orderListDto.UnitPrice,
                        orderListDto.Quantity);
                }
                else
                {
                    order = new Order(orderListDto.Id, customerDto, (OrderStatus)orderListDto.Status,
                        orderListDto.Code, orderListDto.Created, orderListDto.Updated);

                    order.LoadItems(orderListDto.ProductId, orderListDto.ProductName, orderListDto.UnitPrice,
                        orderListDto.Quantity);
                    ordersDictionary.Add(order.Id, order);
                }

                return order;
            },
            splitOn: "Id"
        );

        logger.LogInformation("Retrieved {Count} orders", ordersDictionary.Count);
        return ordersDictionary.Select(d => d.Value).ToList().AsReadOnly();
    }

    [ExcludeFromCodeCoverage(Justification =
        "unit test is not working due to moq.dapper limitations, maybe one day...")]
    public async Task<Order?> GetAsync(Guid orderId)
    {
        logger.LogInformation("Getting order with ID: {OrderId}", orderId);

        var order = await dbConnection.QuerySingleOrDefaultAsync<Order>(
            "select * from Orders where Id = @OrderId",

            param: new { OrderId = orderId });

        logger.LogInformation("Order with ID: {OrderId} retrieved", orderId);
        return order;
    }

    public async Task<bool> UpdateOrderStatusAsync(Order order)
    {
        logger.LogInformation("Updating order {orderId} status", order.Id);

        var updated = await dbConnection.ExecuteAsync(
            "UPDATE Orders SET Status=@Status, Updated=@LastUpdate WHERE Id = @Id",
            new
            {
                order.Status,
                order.LastUpdate,
                order.Id
            }) == 1;

        logger.LogInformation(
            updated ? "Order {orderId} status updated" : "Order {orderId} status not updated", order.Id);

        return updated;
    }
}
