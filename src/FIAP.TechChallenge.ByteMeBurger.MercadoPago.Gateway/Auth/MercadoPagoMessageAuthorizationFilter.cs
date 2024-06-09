// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Security.Cryptography;
using System.Text;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Auth;

/// <summary>
///
/// </summary>
public class MercadoPagoMessageAuthorizationFilter : IAuthorizationFilter
{
    private readonly ILogger<MercadoPagoMessageAuthorizationFilter> _logger;
    private readonly MercadoPagoOptions _mercadoPagoOptions;
    private const string XSignatureHeaderName = "x-signature";
    private const string XRequestIdHeaderName = "x-request-id";

    // private const string Secret = "ccb95076f70b292447d66fffccc22c901316236843633357f3b64fff9502f1a3";

    /// <summary>
    ///
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mercadoPagoOptions"></param>
    public MercadoPagoMessageAuthorizationFilter(ILogger<MercadoPagoMessageAuthorizationFilter> logger,
        IOptions<MercadoPagoOptions> mercadoPagoOptions)
    {
        _logger = logger;
        ArgumentException.ThrowIfNullOrWhiteSpace(mercadoPagoOptions.Value.WebhookSecret,
            nameof(mercadoPagoOptions.Value.WebhookSecret));
        _mercadoPagoOptions = mercadoPagoOptions.Value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            if (!TryValidateRequestParams(context, out var dataId, out var signature, out var xRequestIdHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!TryValidateSignature(context, signature, dataId, xRequestIdHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            ;

            _logger.LogInformation("Webhook message authorized.");
        }
        catch (Exception e)
        {
            _logger.LogError("Webhook message not authorized. Error {@error}", e);
            context.Result = new UnauthorizedResult();
        }
    }

    private bool TryValidateSignature(AuthorizationFilterContext context, StringValues signature, StringValues dataId,
        StringValues xRequestIdHeader)
    {
        var tsHash = signature.ToString().Split(",");
        if (tsHash.Length != 2)
        {
            _logger.LogWarning("Webhook message not authorized. Signature format not valid {signature}",
                signature.ToString());
            return false;
        }

        var ts = tsHash[0].Substring(3, tsHash[0].Length - 3);
        var hash = tsHash[1].Substring(3, tsHash[1].Length - 3);
        var manifest = $"id:{dataId};request-id:{xRequestIdHeader};ts:{ts};";

        var computedSignature = ComputeSignature(manifest, _mercadoPagoOptions.WebhookSecret);
        if (string.Equals(hash, computedSignature, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        _logger.LogWarning("Webhook message not authorized. Invalid signature.");
        return false;
    }

    private bool TryValidateRequestParams(AuthorizationFilterContext context, out StringValues dataId,
        out StringValues signature, out StringValues xRequestIdHeader)
    {
        signature = default;
        xRequestIdHeader = default;

        if (!context.HttpContext.Request.Query.TryGetValue("data_id", out dataId)
            || string.IsNullOrWhiteSpace(dataId))
        {
            _logger.LogWarning("Webhook message not authorized. Missing DataId on {@Request}",
                context.HttpContext.Request);
            return false;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(XSignatureHeaderName, out signature) ||
            string.IsNullOrWhiteSpace(signature))
        {
            _logger.LogWarning("Webhook message not authorized. Missing signature {@header}",
                context.HttpContext.Request.Headers);
            return false;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(XRequestIdHeaderName, out xRequestIdHeader) ||
            string.IsNullOrWhiteSpace(xRequestIdHeader))
        {
            _logger.LogWarning("Webhook message not authorized. Missing RequestId {@header}",
                context.HttpContext.Request.Headers);
            return false;
        }

        return true;
    }

    private static string ComputeSignature(string data, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var computedHmac = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToHexString(computedHmac);
    }
}
