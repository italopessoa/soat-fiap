// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Data;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Persistence.Dto;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Repository;

public class PaymentRepositoryDapper(IDbConnection dbConnection, ILogger<PaymentRepositoryDapper> logger)
    : IPaymentRepository
{
    public async Task<Payment> SaveAsync(Payment payment)
    {
        logger.LogInformation("Persisting Payment with Id: {Id} for Order {OrderId}", payment.Id.Code,
            payment.Id.OrderId);
        dbConnection.Open();

        var transaction = dbConnection.BeginTransaction();
        {
            try
            {
                var paymentDao = new PaymentDAO(payment.Id.Code, payment.Id.OrderId, (int)payment.Status,
                    (int)payment.PaymentType, payment.Amount, payment.Created, null);
                await dbConnection.ExecuteAsync(Constants.InsertPaymentQuery, paymentDao);

                transaction.Commit();

                logger.LogInformation("Payment {PaymentId} persisted", payment.Id.Code);
                return payment;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error Persisting Payment with Id: {Id} for Order {OrderId}", payment.Id.Code,
                    payment.Id.Code);
                transaction.Rollback();
                throw;
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }

    public async Task<Payment?> GetPaymentAsync(string paymentId)
    {
        logger.LogInformation("Getting Payment with ID: {PaymentId}", paymentId);

        var paymentDao = await dbConnection.QuerySingleOrDefaultAsync<PaymentDAO>(
            Constants.GetPaymentQuery,
            param: new { Id = paymentId }
        );

        if (paymentDao is null)
        {
            return null;
        }

        logger.LogInformation("Payment with ID: {PaymentId} retrieved", paymentId);
        return new Payment()
        {
            Id = new PaymentId(paymentDao.Id, paymentDao.OrderId),
            PaymentType = (PaymentType)paymentDao.PaymentType,
            // TODO QrCode = paymentDao.QrCode
            Amount = paymentDao.Amount,
            Created = paymentDao.Created,
            Status = (PaymentStatus)paymentDao.Status
        };
    }

    public async Task<bool> UpdatePaymentStatusAsync(Payment payment)
    {
        logger.LogInformation("Updating Payment {PaymentId} status", payment.Id.Code);

        var updated = await dbConnection.ExecuteAsync(
            Constants.UpdatePaymentStatusQuery,
            new
            {
                Id = payment.Id.Code,
                Status = (int)payment.Status,
                payment.Updated
            }) == 1;

        logger.LogInformation(
            updated ? "Payment {PaymentId} status updated" : "Payment {PaymentId} status not updated", payment.Id.Code);

        return updated;
    }
}
