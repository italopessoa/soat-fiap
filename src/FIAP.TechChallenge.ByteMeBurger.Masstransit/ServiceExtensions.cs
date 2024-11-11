using System.Diagnostics.CodeAnalysis;
using Bmb.Domain.Core.Events;
using Bmb.Domain.Core.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Masstransit.Consumers;
using FIAP.TechChallenge.ByteMeBurger.Masstransit.Factory;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FIAP.TechChallenge.ByteMeBurger.Masstransit;

[ExcludeFromCodeCoverage]
public class SqsSettingsSetup(IConfiguration configuration) : IConfigureOptions<SqsSettings>
{
    public void Configure(SqsSettings options)
    {
        configuration
            .GetSection(nameof(SqsSettings))
            .Bind(options);
    }
}

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static void ConfigureAnalyticsService(this IServiceCollection services)
    {
        services.ConfigureOptions<SqsSettingsSetup>();
        services.AddSingleton<ISqsClientFactory, SqsClientFactory>();
        services.AddSingleton<IAnalyticsPublisher, SqsService>();
        services.AddSingleton<IAnalyticsPublisher2, EventsDispatcher>();
    }

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
