using System.Data;
using AutoFixture;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Persistence.Dto;
using FIAP.TechChallenge.ByteMeBurger.Persistence.Repository;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Dapper;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Test.Repository;

[TestSubject(typeof(PaymentRepositoryDapper))]
public class PaymentRepositoryDapperTest
{
    private readonly Mock<IDbConnection> _mockConnection;
    private readonly PaymentRepositoryDapper _target;

    public PaymentRepositoryDapperTest()
    {
        Mock<IDbContext> mockDbContext = new();
        _mockConnection = new Mock<IDbConnection>();
        _mockConnection.Setup(c => c.BeginTransaction()).Returns(Mock.Of<IDbTransaction>());
        mockDbContext.Setup(s => s.CreateConnection())
            .Returns(_mockConnection.Object);
        _target = new PaymentRepositoryDapper(mockDbContext.Object, Mock.Of<ILogger<PaymentRepositoryDapper>>());
    }

    [Fact]
    public async Task Create_Success()
    {
        // Arrange
        var expectedPayment = new Fixture().Create<Payment>();

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

    [Fact]
    public async Task GetPaymentAsync_Success()
    {
        // Arrange
        var expectedPaymentDao = new Fixture()
            .Build<PaymentDto>()
            .With(p => p.PaymentType, (int)PaymentType.MercadoPago)
            .With(p => p.Status, (int)PaymentStatus.Paid)
            .Create();

        _mockConnection.SetupDapperAsync(c =>
                c.QuerySingleOrDefaultAsync<PaymentDto>(Constants.GetPaymentQuery, null, null, null, null))
            .ReturnsAsync(expectedPaymentDao);

        // Act
        var result = await _target.GetPaymentAsync(new PaymentId(expectedPaymentDao.Id));

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Status.Should().Be(PaymentStatus.Paid);
        }
    }
}
