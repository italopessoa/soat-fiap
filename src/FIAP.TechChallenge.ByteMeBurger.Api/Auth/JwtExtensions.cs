using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Auth;

public static class JwtExtensions
{
    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration
            .GetSection("JwtOptions")
            .Get<JwtOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
}
