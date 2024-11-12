using System.Diagnostics.CodeAnalysis;
using Bmb.Domain.Core.Events;
using FIAP.TechChallenge.ByteMeBurger.Masstransit.Consumers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.Masstransit;

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static void ConfigureDispatcher(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderStatusChangedConsumer>();
            x.AddConsumer<OrderPaymentCreatedConsumer>();
            x.AddConsumer<OrderPaymentApprovedConsumer>();
            x.UsingAmazonSqs((context, cfg) =>
            {
                cfg.Host("us-east-1", h =>
                {
                    // h.Scope("dev", true);
                });
                cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter( false));
            });
        });
        services.AddScoped<IDispatcher, Dispatcher>();
    }
}
