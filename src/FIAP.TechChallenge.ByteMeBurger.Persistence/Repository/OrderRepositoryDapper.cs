// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Persistence.Dto;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Repository;

public class OrderRepositoryDapper(IDbConnection dbConnection, ILogger<OrderRepositoryDapper> logger)
    : IOrderRepository
{
    public async Task<Order> CreateAsync(Order order)
    {
        logger.LogInformation("Persisting order with ID: {OrderId}", order.Id);
        dbConnection.Open();
        var transaction = dbConnection.BeginTransaction();
        {
            try
            {
                await dbConnection.ExecuteAsync(
                    Constants.InsertOrderQuery,
                    new
                    {
                        Id = order.Id,
                        CustomerId = order.Customer?.Id,
                        Status = (int)order.Status,
                        Created = order.Created,
                        TrackingCode = order.TrackingCode.Value
                    });
                await dbConnection.ExecuteAsync(Constants.InsertOrderItemsQuery, order.OrderItems);

                transaction.Commit();
                logger.LogInformation("Order with ID: {OrderId} persisted", order.Id);
                return order;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error persisting order with ID: {OrderId}", order.Id);
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

        await dbConnection.QueryAsync<OrderListDto, CustomerDto, PaymentDAO?, OrderItemDto, Order>(
            Constants.GetAllOrdersQuery,
            (orderListDto, customerDto, paymentDao, orderItemDto) =>
            {
                if (ordersDictionary.TryGetValue(orderListDto.Id, out var order))
                {
                    order.LoadItems(orderItemDto.ProductId, orderItemDto.ProductName, orderItemDto.UnitPrice,
                        orderItemDto.Quantity);
                }
                else
                {
                    order = new Order(orderListDto.Id, customerDto, (OrderStatus)orderListDto.Status,
                        new OrderTrackingCode(orderListDto.TrackingCode), orderListDto.Created, orderListDto.Updated);

                    if (paymentDao is not null)
                        order.PaymentId = new PaymentId(paymentDao.Id, paymentDao.OrderId);

                    order.LoadItems(orderItemDto.ProductId, orderItemDto.ProductName, orderItemDto.UnitPrice,
                        orderItemDto.Quantity);
                    ordersDictionary.Add(order.Id, order);
                }

                return order;
            },
            splitOn: "Id, ProductId"
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

        await dbConnection.QueryAsync<OrderListDto, CustomerDto, PaymentDAO?, OrderItemDto, Order>(
            Constants.GetOrderByIdQuery,
            (orderListDto, customerDto, paymentDao, orderItemDto) =>
            {
                if (ordersDictionary.TryGetValue(orderListDto.Id, out var order))
                {
                    order.LoadItems(orderItemDto.ProductId, orderItemDto.ProductName, orderItemDto.UnitPrice,
                        orderItemDto.Quantity);
                }
                else
                {
                    order = new Order(orderListDto.Id, customerDto, (OrderStatus)orderListDto.Status,
                        new OrderTrackingCode(orderListDto.TrackingCode), orderListDto.Created, orderListDto.Updated);

                    if (paymentDao is not null)
                        order.PaymentId = new PaymentId(paymentDao.Id, paymentDao.OrderId);

                    order.LoadItems(orderItemDto.ProductId, orderItemDto.ProductName, orderItemDto.UnitPrice,
                        orderItemDto.Quantity);
                    ordersDictionary.Add(order.Id, order);
                }

                return order;
            },
            param: new { OrderId = orderId },
            splitOn: "Id, ProductId"
        );


        logger.LogInformation("Order with ID: {OrderId} retrieved", orderId);
        return ordersDictionary.Any() ? ordersDictionary.First().Value : null;
    }

    public async Task<bool> UpdateOrderStatusAsync(Order order)
    {
        logger.LogInformation("Updating order {orderId} status", order.Id);
        try
        {
            var updated = await dbConnection.ExecuteAsync(
                Constants.UpdateOrderStatusQuery,
                new
                {
                    order.Status,
                    Updated = order.LastUpdate,
                    order.Id
                }) == 1;

            logger.LogInformation(
                updated ? "Order {orderId} status updated" : "Order {orderId} status not updated", order.Id);
            return true;
        }
        catch (Exception e)
        {
            logger.LogError("Error when trying to Update Order {OrderId}. Details {@Exception}", order.Id, e);
            return false;
        }
    }
}
