// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;
using MercadoPago.Client;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DomainPayment = FIAP.TechChallenge.ByteMeBurger.Domain.Entities.Payment;
using DomainPaymentStatus = FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects.PaymentStatus;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway;

[ExcludeFromCodeCoverage]
public class MercadoPagoService : IPaymentGateway
{
    private readonly ILogger<MercadoPagoService> _logger;
    private readonly MercadoPagoOptions _mercadoPagoOptions;
    private const decimal IntegrationPrice = 0.01M;

    public MercadoPagoService(IOptions<MercadoPagoOptions> mercadoPagoOptions, ILogger<MercadoPagoService> logger)
    {
        _logger = logger;
        ArgumentException.ThrowIfNullOrWhiteSpace(mercadoPagoOptions.Value.WebhookSecret,
            nameof(mercadoPagoOptions.Value.WebhookSecret));
        _mercadoPagoOptions = mercadoPagoOptions.Value;
    }

    public async Task<DomainPayment?> CreatePaymentAsync(Order order)
    {
        var requestOptions = new RequestOptions()
        {
            AccessToken = _mercadoPagoOptions.AccessToken,
            CustomHeaders =
            {
                { "x-idempotency-key", order.Id.ToString() }
            }
        };

        var paymentCreateRequest = GetPaymentCreateRequest(order);
        var client = new PaymentClient();
        try
        {
            var mercadoPagoPayment = await client.CreateAsync(paymentCreateRequest, requestOptions);
            var status = Enum.TryParse(typeof(PaymentStatus), mercadoPagoPayment.Status, true, out var paymentStatus)
                ? (PaymentStatus)paymentStatus
                : PaymentStatus.Pending;

            _logger.LogInformation("Trying to create new payment on MercadoPago for Order {OrderId}", order.Id);
            return new DomainPayment
            {
                Status = status,
                Id = new PaymentId(mercadoPagoPayment.Id.ToString()!, order.Id),
                PaymentType = PaymentType.MercadoPago,
                QrCode = mercadoPagoPayment.PointOfInteraction.TransactionData.QrCode,
            };
        }
        catch (Exception e)
        {
            _logger.LogError("Error when trying to create new payment on MercadoPago for Order {OrderId}. {@Error}",
                order.Id, e);
            return null;
        }
    }

    private PaymentCreateRequest GetPaymentCreateRequest(Order order)
    {
        var paymentPayerRequest = order.Customer is null
            ? new PaymentPayerRequest
            {
                Email = "guest@mercadofiado.com",
            }
            : MapPaymentPayerRequest(order);

        var items = MapPaymentItemRequests(order).ToList();

        var totalAmount = items.Sum(i => i.Quantity * i.UnitPrice);
        var additionalInfo = new PaymentAdditionalInfoRequest
        {
            Items = items.ToList(),
        };

        return MapPaymentCreateRequest(order, paymentPayerRequest, additionalInfo, totalAmount!.Value);
    }

    public async Task<DomainPaymentStatus?> GetPaymentStatusAsync(string paymentId)
    {
        var requestOptions = new RequestOptions()
        {
            AccessToken = _mercadoPagoOptions.AccessToken,
        };
        var client = new PaymentClient();
        var payment = await client.GetAsync(long.Parse(paymentId), requestOptions);

        return payment?.Status switch
        {
            global::MercadoPago.Resource.Payment.PaymentStatus.Approved => DomainPaymentStatus.Approved,
            global::MercadoPago.Resource.Payment.PaymentStatus.Pending => DomainPaymentStatus.Pending,
            global::MercadoPago.Resource.Payment.PaymentStatus.InProcess => DomainPaymentStatus.InProgress,
            global::MercadoPago.Resource.Payment.PaymentStatus.Rejected => DomainPaymentStatus.Rejected,
            global::MercadoPago.Resource.Payment.PaymentStatus.Cancelled => DomainPaymentStatus.Cancelled,
            _ => null
        };
    }

    private static PaymentPayerRequest MapPaymentPayerRequest(Order order)
        => new()
        {
            Email = order.Customer.Email,
            FirstName = order.Customer.Name,
            LastName = "User",
            Identification = new IdentificationRequest
            {
                Type = "CPF",
                Number = order.Customer.Cpf
            },
        };

    private static IEnumerable<PaymentItemRequest> MapPaymentItemRequests(Order order)
        => order.OrderItems.Select(item =>
            new PaymentItemRequest
            {
                Id = item.Id.ToString(),
                Title = item.ProductName,
                Description = item.ProductName,
                CategoryId = "food",
                Quantity = item.Quantity,
                UnitPrice = IntegrationPrice,
                Warranty = false,
                CategoryDescriptor = new PaymentCategoryDescriptorRequest
                {
                    Passenger = { },
                    Route = { }
                }
            }
        );

    private PaymentCreateRequest MapPaymentCreateRequest(Order order, PaymentPayerRequest payer,
        PaymentAdditionalInfoRequest paymentAdditionalInfoRequest, decimal amount)
        => new()
        {
            Description = $"Payment for Order {order.TrackingCode.Value}",
            ExternalReference = order.TrackingCode.Value,
            Installments = 1,
            NotificationUrl = _mercadoPagoOptions.NotificationUrl ?? string.Empty,
            Payer = payer,
            PaymentMethodId = "pix",
            StatementDescriptor = "tech challenge restaurant order",
            TransactionAmount = amount,
            AdditionalInfo = paymentAdditionalInfoRequest,
            DateOfExpiration = DateTime.UtcNow.AddMinutes(15)
        };
}
