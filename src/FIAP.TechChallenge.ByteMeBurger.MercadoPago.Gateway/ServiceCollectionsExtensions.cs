using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionsExtensions
{
    public static void AddMercadoPagoGateway(this IServiceCollection services)
    {
        services.AddSingleton<MercadoPagoOptions>(provider =>
        {
            var configuration = provider.GetService<IConfiguration>();
            var options = new MercadoPagoOptions();
            configuration.GetSection(MercadoPagoOptions.MercadoPago)
                .Bind(options);

            return options;
        });

        services.AddSingleton<IMercadoPagoHmacSignatureValidator, MercadoPagoHmacSignatureValidator>()
            .AddKeyedScoped<IPaymentGateway, MercadoPagoService>($"Payment-{nameof(PaymentType.MercadoPago)}");
    }
}
