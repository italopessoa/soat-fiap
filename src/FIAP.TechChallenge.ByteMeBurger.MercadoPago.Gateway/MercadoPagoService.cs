// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;
using MercadoPago.Client;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using Microsoft.Extensions.Options;
using DomainPayment = FIAP.TechChallenge.ByteMeBurger.Domain.Entities.Payment;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway;

public class MercadoPagoService : IPaymentGateway
{
    private readonly MercadoPagoOptions _mercadoPagoOptions;

    public MercadoPagoService(IOptions<MercadoPagoOptions> mercadoPagoOptions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(mercadoPagoOptions.Value.WebhookSecret,
            nameof(mercadoPagoOptions.Value.WebhookSecret));
        _mercadoPagoOptions = mercadoPagoOptions.Value;
    }

    public async Task<DomainPayment> CreatePaymentAsync(Order order)
    {
        var paymentPayerRequest = new PaymentPayerRequest()
        {
            Email = "guest@mercadofiado.com",
        };
        var items = order.OrderItems.Select(item =>
            new PaymentItemRequest
            {
                Id = item.Id.ToString(),
                Title = item.ProductName,
                Description = item.ProductName,
                // PictureUrl =
                // "https://http2.mlstatic.com/resources/frontend/statics/growth-sellers-landings/device-mlb-point-i_medium2x.png",
                CategoryId = "food",
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Warranty = false,
                CategoryDescriptor = new PaymentCategoryDescriptorRequest
                {
                    Passenger = { },
                    Route = { }
                }
            }
        );

        // var payerInfo = new PaymentAdditionalInfoPayerRequest
        // {
        //     FirstName = "Test",
        //     LastName = "Test",
        //     Phone = new PhoneRequest
        //     {
        //         AreaCode = "11",
        //         Number = "987654321"
        //     },
        //     Address = new AddressRequest
        //     {
        //         StreetNumber = null
        //     }
        // };

        var additionalInfo = new PaymentAdditionalInfoRequest
        {
            Items = items.ToList(),
            // Payer = payerInfo,
        };

        if (order.Customer is not null)
        {
            paymentPayerRequest = new PaymentPayerRequest()
            {
                Type = "individual",
                Email = order.Customer.Email,
                FirstName = order.Customer.Name,
                LastName = "User",
                Identification = new IdentificationRequest
                {
                    Type = "CPF",
                    Number = order.Customer.Cpf
                },
            };
        }

        var request = new PaymentCreateRequest
        {
            Description = $"Payment for Order {order.TrackingCode.Value}",
            ExternalReference = order.TrackingCode.Value,
            Installments = 1,
            NotificationUrl = _mercadoPagoOptions.NotificationUrl,
            Payer = paymentPayerRequest,
            PaymentMethodId = "pix",
            StatementDescriptor = "statement descriptor",
            TransactionAmount = (decimal?)0.01,
            AdditionalInfo = additionalInfo,
            DateOfExpiration = DateTime.UtcNow.AddMinutes(5)
        };

        var client = new PaymentClient();
        var requestOptions = new RequestOptions()
        {
            AccessToken = _mercadoPagoOptions.AccessToken,
            CustomHeaders =
            {
                { "x-idempotency-key", Guid.NewGuid().ToString() }
            }
        };

        try
        {
            var mercadoPagoPayment = await client.CreateAsync(request, requestOptions);
            return new DomainPayment
            {
                Status = mercadoPagoPayment.Status,
                OrderId = order.Id,
                SystemId = "mercadopago"
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
