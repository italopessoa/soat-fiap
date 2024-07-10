// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Api.Controllers;
using FIAP.TechChallenge.ByteMeBurger.Api.Model.Payment;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using DomainPaymentType = FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects.PaymentType;
using PaymentType = FIAP.TechChallenge.ByteMeBurger.Api.Model.Payment.PaymentType;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Test.Controllers;

[TestSubject(typeof(PaymentsController))]
public class PaymentsControllerTest
{
    private readonly Mock<IPaymentService> _serviceMock;
    private readonly PaymentsController _target;

    public PaymentsControllerTest()
    {
        _serviceMock = new Mock<IPaymentService>();
        _target = new PaymentsController(_serviceMock.Object);
    }

    [Fact]
    public async void Create_Success()
    {
        // Arrange
        var paymentId = new PaymentId("123", Guid.NewGuid());
        var payment = new Payment(paymentId, "qrcode", 10);
        _serviceMock.Setup(p => p.CreateOrderPaymentAsync(It.IsAny<CreateOrderPaymentRequestDto>()))
            .ReturnsAsync(payment);
        var paymentRequest = new CreatePaymentRequest
        {
            OrderId = Guid.NewGuid(),
            PaymentType = PaymentType.Test
        };

        // Act
        var response = await _target.Create(paymentRequest, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<CreatedResult>();
            var paymentViewModel = response.Result.As<CreatedResult>().Value.As<PaymentViewModel>();

            paymentViewModel.PaymentId.Should().Be(payment.Id.Code);
            paymentViewModel.QrCode.Should().Be(payment.QrCode);
        }
    }

    [Fact]
    public async void GetStatus_Success()
    {
        // Arrange
        var paymentId = new PaymentId("123", Guid.NewGuid());
        var payment = new Payment(paymentId, "qrcode", 10, DomainPaymentType.MercadoPago)
        {
            Status = PaymentStatus.Paid
        };
        _serviceMock.Setup(p => p.GetPaymentAsync(It.IsAny<string>()))
            .ReturnsAsync(payment);

        // Act
        var response = await _target.GetStatus(paymentId.Code, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var status = response.Result.As<OkObjectResult>().Value.As<PaymentStatusViewModel>();

            status.Should().Be(PaymentStatusViewModel.Paid);
        }
    }
}
