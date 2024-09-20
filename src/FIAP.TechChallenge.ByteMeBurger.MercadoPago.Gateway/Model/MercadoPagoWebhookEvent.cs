using System.Text.Json.Serialization;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Model;

public class MercadoPagoWebhookEvent
{
    public string Action { get; set; } = null!;

    [JsonPropertyName("api_version")]
    public string ApiVersion { get; set; } = null!;

    public MercadoPagoWebhookData Data { get; set; }

    [JsonPropertyName("date_created")]
    public DateTime DateCreated { get; set; }

    public long Id { get; set; }

    [JsonPropertyName("live_mode")]
    public bool LiveMode { get; set; }

    public string Type { get; set; } = null!;

    [JsonPropertyName("user_id")]
    public long UserId { get; set; }
}

public class MercadoPagoWebhookData
{
    public string Id { get; set; } = null!;
}
