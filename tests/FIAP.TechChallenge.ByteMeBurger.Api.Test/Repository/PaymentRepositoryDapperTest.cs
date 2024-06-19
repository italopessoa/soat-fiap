// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Data;
using AutoFixture;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Persistence;
using FIAP.TechChallenge.ByteMeBurger.Persistence.Repository;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Dapper;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Test.Repository;

[TestSubject(typeof(PaymentRepositoryDapper))]
public class PaymentRepositoryDapperTest
{
    private readonly Mock<IDbConnection> _mockConnection;
    private readonly PaymentRepositoryDapper _target;

    public PaymentRepositoryDapperTest()
    {
        _mockConnection = new Mock<IDbConnection>();
        _target = new PaymentRepositoryDapper(_mockConnection.Object, Mock.Of<ILogger<PaymentRepositoryDapper>>());
    }

    [Fact]
    public async Task Create_Success()
    {
        // Arrange
        var expectedPayment = new Fixture().Create<Payment>();

        _mockConnection.Setup(c => c.BeginTransaction()).Returns(Mock.Of<IDbTransaction>());

        _mockConnection.SetupDapperAsync(c =>
                c.ExecuteAsync(Constants.InsertPaymentQuery,
                    null, null, null, null))
            .ReturnsAsync(1);

        // Act
        var result = await _target.SaveAsync(expectedPayment);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().Be(expectedPayment);
        }
    }
}
