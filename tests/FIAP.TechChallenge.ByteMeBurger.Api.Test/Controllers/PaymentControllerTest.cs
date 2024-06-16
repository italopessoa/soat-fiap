// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Api.Controllers;
using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Test.Controllers;

[TestSubject(typeof(PaymentController))]
public class PaymentControllerTest
{
    private readonly Mock<IPaymentService> _serviceMock;
    private readonly PaymentController _target;

    public PaymentControllerTest()
    {
        _serviceMock = new Mock<IPaymentService>();
        _target = new PaymentController(_serviceMock.Object);
    }

    [Fact]
    public async void Create_Success()
    {
        // Arrange
        var paymentId = new PaymentId("123", 1);
        var payment = new Payment(paymentId, Guid.NewGuid(), "qrcode");
        _serviceMock.Setup(p => p.CreateOrderPaymentAsync(It.IsAny<Guid>()))
            .ReturnsAsync(payment);

        // Act
        var response = await _target.Create(Guid.NewGuid(), CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var paymentViewModel = response.Result.As<OkObjectResult>().Value.As<PaymentViewModel>();

            paymentViewModel.PaymentId.Should().Be(payment.Id.ExternalReference);
            paymentViewModel.VendorId.Should().Be(payment.Id.SystemId.ToString());
            paymentViewModel.QrCode.Should().Be(payment.QrCode);
        }
    }
}
