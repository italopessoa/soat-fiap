using System.Text.Json.Serialization;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Model;

public class MercadoPagoWebhookEvent
{
    public string Action { get; set; } = string.Empty;

    [JsonPropertyName("api_version")]
    public string ApiVersion { get; set; } = string.Empty;

    public MercadoPagoWebhookData Data { get; set; }

    [JsonPropertyName("date_created")]
    public DateTime DateCreated { get; set; }

    public long Id { get; set; }

    [JsonPropertyName("live_mode")]
    public bool LiveMode { get; set; }

    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("user_id")]
    public long UserId { get; set; }
}

public class MercadoPagoWebhookData
{
    public string Id { get; set; } = string.Empty;
}
