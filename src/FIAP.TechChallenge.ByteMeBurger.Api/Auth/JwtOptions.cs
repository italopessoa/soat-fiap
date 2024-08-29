namespace FIAP.TechChallenge.ByteMeBurger.Api.Auth;

public record JwtOptions(
    string Issuer,
    string Audience,
    string SigningKey,
    int ExpirationSeconds,
    bool UseAccessToken
);
