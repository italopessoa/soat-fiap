using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.DI;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    public static void ConfigureApiDependencyInversion(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureApplicationDI(builder.Configuration);
    }
}
