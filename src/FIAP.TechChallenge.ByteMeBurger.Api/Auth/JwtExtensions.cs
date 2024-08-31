using System.Security.Claims;
using System.Text;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using Microsoft.IdentityModel.Tokens;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Auth;

/// <summary>
/// Jwt token extensions methods
/// </summary>
public static class JwtExtensions
{
    /// <summary>
    /// Configure Jtw token validation
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration
            .GetSection("JwtOptions")
            .Get<JwtOptions>();

        services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                if (jwtOptions.UseAccessToken)
                {
                    options.Events = AccessTokenAuthEventsHandler.Instance;
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    LogValidationExceptions = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
                };
            });
    }

    // https://stackoverflow.com/a/55740879/2921329
    /// <summary>
    /// Get customer details from Jwt Claims
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static CustomerDto? GetCustomerFromClaims(this HttpContext context)
    {
        if (Guid.TryParse(
                context.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.ToString(),
                out var customerId))
        {
            var email = context.User.Claims.First(claim => claim.Type == ClaimTypes.Email).ToString();
            var name = context.User.Claims.First(claim => claim.Type == ClaimTypes.Name).ToString();
            var cpf = context.User.Claims.First(claim => claim.Type == "cpf").ToString();

            return new CustomerDto(customerId, cpf, name, email);
        }

        return default;
    }
}
