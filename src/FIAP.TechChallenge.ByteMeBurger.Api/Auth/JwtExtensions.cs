using System.Security.Claims;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
namespace FIAP.TechChallenge.ByteMeBurger.Api.Auth;

/// <summary>
/// Jwt token extensions methods
/// </summary>
public static class JwtExtensions
{
    // https://stackoverflow.com/a/55740879/2921329
    /// <summary>
    /// Get customer details from Jwt Claims
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static CustomerDto? GetCustomerFromClaims(this HttpContext context)
    {
        if (Guid.TryParse(
                context.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value,
                out var customerId))
        {
            var email = context.User.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
            var name = context.User.Claims.First(claim => claim.Type is ClaimTypes.Name or "name" ).Value;
            var cpf = context.User.Claims.First(claim => claim.Type == "cpf").Value;

            return new CustomerDto(customerId, cpf, name, email);
        }

        return default;
    }
}
