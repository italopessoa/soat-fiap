using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Auth;

/// <summary>
/// Singleton class handler of events related to JWT authentication
/// </summary>
internal class AccessTokenAuthEventsHandler : JwtBearerEvents
{
    private const string BearerPrefix = "Bearer ";

    private AccessTokenAuthEventsHandler() => OnMessageReceived = MessageReceivedHandler;

    /// <summary>
    /// Gets single available instance of <see cref="AccessTokenAuthEventsHandler"/>
    /// </summary>
    public static AccessTokenAuthEventsHandler Instance { get; } = new AccessTokenAuthEventsHandler();

    private Task MessageReceivedHandler(MessageReceivedContext context)
    {
        if (context.Request.Headers.TryGetValue("AccessToken", out var headerValue))
        {
            string token = headerValue;
            if (!string.IsNullOrEmpty(token) && token.StartsWith(BearerPrefix))
            {
                token = token.Substring(BearerPrefix.Length);
            }

            context.Token = token;
        }

        return Task.CompletedTask;
    }
}
