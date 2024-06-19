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
                    payment.PaymentType, payment.Amount, payment.Created, null);
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

    public Task<PaymentStatus> GetPaymentStatusAsync(string paymentId)
    {
        throw new NotImplementedException();
    }
}
