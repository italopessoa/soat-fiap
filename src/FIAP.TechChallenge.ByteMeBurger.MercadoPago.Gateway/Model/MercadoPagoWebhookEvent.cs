// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Text.Json.Serialization;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Model;

public class MercadoPagoWebhookEvent
{
    public string Action { get; set; }

    [JsonPropertyName("api_version")] public string ApiVersion { get; set; }

    public MercadoPagoWebhookData Data { get; set; }

    [JsonPropertyName("date_created")] public DateTime DateCreated { get; set; }

    public long Id { get; set; }

    [JsonPropertyName("live_mode")] public bool LiveMode { get; set; }

    public string Type { get; set; }

    [JsonPropertyName("user_id")] public long UserId { get; set; }
}

public class MercadoPagoWebhookData
{
    public string Id { get; set; }
}
