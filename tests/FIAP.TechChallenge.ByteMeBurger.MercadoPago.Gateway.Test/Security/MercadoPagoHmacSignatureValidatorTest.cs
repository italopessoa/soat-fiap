// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Security;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Test.Security;

[TestSubject(typeof(MercadoPagoHmacSignatureValidator))]
public class MercadoPagoHmacSignatureValidatorTest
{
    private readonly IOptions<MercadoPagoOptions> _options = Options.Create(new MercadoPagoOptions
    {
        WebhookSecret = "test_secret"
    });

    [Fact]
    public void Validate_Constructor()
    {
        // Arrange
        var validator = () => new MercadoPagoHmacSignatureValidator(Options.Create(
            new MercadoPagoOptions()));

        // Act& Assert
        using (new AssertionScope())
        {
            validator.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should()
                .Be(nameof(MercadoPagoOptions.WebhookSecret));
        }
    }

    [Theory]
    [InlineData("data_id", "request_id", "signature", "Signature format not valid")]
    [InlineData("", "request_id", "signature", "Missing DataId")]
    [InlineData("data_id", "", "signature", "Missing RequestId")]
    [InlineData("data_id", "request_id", "signatureS", "Signature format not valid")]
    public void OnAuthorization_MissingValues_ReturnsUnauthorized(string dataId, string requestId, string signature,
        string missingParam)
    {
        // Arrange
        var validator = new MercadoPagoHmacSignatureValidator(_options);

        var httpContext = new DefaultHttpContext
        {
            Request =
            {
                Headers =
                {
                    ["x-signature"] = signature,
                    ["x-request-id"] = requestId,
                },
                Query = new QueryCollection(new Dictionary<string, StringValues>
                {
                    { "data_id", dataId }
                })
            },
        };

        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var context = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

        // Act & Assert
        using (new AssertionScope())
        {
            validator.TryToValidate(context, out var reasonToFail)
                .Should()
                .BeFalse();
            reasonToFail.Should().Contain(missingParam);
        }
    }

    [Fact]
    public void OnAuthorization_Exception_ReturnsUnauthorized()
    {
        // Arrange
        var validator = new MercadoPagoHmacSignatureValidator(_options);
        var httpContext = new DefaultHttpContext
        {
            Request =
            {
                Headers =
                {
                    ["x-signature"] = "ts=123,",
                    ["x-request-id"] = "requestId",
                },
                Query = new QueryCollection(new Dictionary<string, StringValues>
                {
                    { "data_id", "data_id" }
                })
            },
        };

        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var context = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

        //  Act & Assert
        using (new AssertionScope())
        {
            validator.TryToValidate(context, out var reasonToFail)
                .Should()
                .BeFalse();
            reasonToFail.Should().NotBeEmpty();
        }
    }
}
