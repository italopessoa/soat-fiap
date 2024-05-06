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
                     left join Customers c on c.Id = o.CustomerId",
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
        var ordersDictionary = new Dictionary<Guid, Order>();

        await dbConnection.QueryAsync<Order, OrderItem, Order>(
            "select * from Orders o inner join OrderItems oi on oi.OrderId = o.Id where o.Id = @OrderId",
            (order, orderItem) =>
            {
                if (ordersDictionary.TryGetValue(order.Id, out var existingOrder))
                    order = existingOrder;
                else
                    ordersDictionary.Add(order.Id, order);

                order.AddOrderItem(orderItem.ProductId, orderItem.ProductName, orderItem.UnitPrice, orderItem.Quantity);

                return order;
            },
            splitOn: "CustomerId",
            param: new { OrderId = orderId });

        logger.LogInformation("Order with ID: {OrderId} retrieved", orderId);
        return ordersDictionary.Select(x => x.Value).FirstOrDefault();
    }
}