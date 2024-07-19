using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.FakePayment.Gateway;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Test;

[TestSubject(typeof(PaymentGatewayFactory))]
public class PaymentGatewayFactoryTest
{
    private readonly IServiceCollection _serviceCollection;

    public PaymentGatewayFactoryTest()
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.Configure<MercadoPagoOptions>(options =>
        {
            options.AccessToken = "YourAccessToken";
            options.NotificationUrl = "YourNotificationUrl";
            options.WebhookSecret = "YourWebhookSecret";
        });

        var mockLogger = new Mock<ILogger<MercadoPagoService>>();
        _serviceCollection.AddSingleton(mockLogger.Object);
    }

    [Fact]
    public void Create_MercadoPago_ReturnsCorrectService()
    {
        // Arrange
        _serviceCollection.AddKeyedScoped<IPaymentGateway, MercadoPagoService>(
            $"Payment-{nameof(PaymentType.MercadoPago)}");
        var serviceProvider = _serviceCollection.BuildServiceProvider();
        var factory = new PaymentGatewayFactory(serviceProvider);

        // Act
        var result = factory.Create(PaymentType.MercadoPago);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<MercadoPagoService>();
        }
    }

    [Fact]
    public void Create_Test_ReturnsCorrectService()
    {
        // Arrange
        _serviceCollection.AddKeyedScoped<IPaymentGateway, FakePaymentGatewayService>(
            $"Payment-{nameof(PaymentType.Test)}");
        var serviceProvider = _serviceCollection.BuildServiceProvider();
        var factory = new PaymentGatewayFactory(serviceProvider);

        // Act
        var result = factory.Create(PaymentType.Test);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<FakePaymentGatewayService>();
        }
    }

    [Fact]
    public void Create_InvalidPaymentType_ThrowsException()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var factory = new PaymentGatewayFactory(serviceProvider);
        var func = () => factory.Create((PaymentType)999);

        // Act & Assert
        func.Should().ThrowExactly<DomainException>();
    }
}
