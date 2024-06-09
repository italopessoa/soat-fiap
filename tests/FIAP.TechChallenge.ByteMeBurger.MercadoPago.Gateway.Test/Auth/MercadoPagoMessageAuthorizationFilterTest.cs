// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Auth;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Test.Auth;

[TestSubject(typeof(MercadoPagoMessageAuthorizationFilter))]
public class MercadoPagoMessageAuthorizationFilterTest
{
    public class MercadoPagoMessageAuthorizationFilterTests
    {
        private MercadoPagoMessageAuthorizationFilter? _filter;
        private readonly Mock<ILogger<MercadoPagoMessageAuthorizationFilter>> _loggerMock = new();

        private readonly IOptions<MercadoPagoOptions> _options = Options.Create(new MercadoPagoOptions
        {
            WebhookSecret = "test_secret"
        });


        [Fact]
        public void Validate_Constructor()
        {
            // Arrange
            var filter = () => new MercadoPagoMessageAuthorizationFilter(_loggerMock.Object, Options.Create(
                new MercadoPagoOptions()));

            // Act& Assert
            using (new AssertionScope())
            {
                filter.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should()
                    .Be(nameof(MercadoPagoOptions.WebhookSecret));
            }
        }

        [Theory]
        [InlineData("data_id", "request_id", "signature", "Signature format not valid")]
        [InlineData("", "request_id", "signature", "Missing DataId")]
        [InlineData("data_id", "", "signature", "Missing RequestId")]
        [InlineData("data_id", "request_id", "signatureS", "Signature format not valid")]
        [InlineData("data_id", "request_id", "ts=123,", "Error")]
        public void OnAuthorization_MissingValues_ReturnsUnauthorized(string dataId, string requestId, string signature,
            string missingParam)
        {
            // Arrange
            List<string> logMessages = [];

            _filter = new MercadoPagoMessageAuthorizationFilter(_loggerMock.Object, _options);
            _loggerMock.Setup(
                    x => x.Log(
                        It.IsAny<LogLevel>(),
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        It.IsAny<Exception>(),
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logMessage = invocation.Arguments[2].ToString();
                    if (logMessage != null) logMessages.Add(logMessage);
                }));

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

            // Act
            _filter.OnAuthorization(context);

            // Assert
            using (new AssertionScope())
            {
                Assert.IsType<UnauthorizedResult>(context.Result);
                logMessages.Any(m => m.Contains(missingParam)).Should().BeTrue("{0} should be logged", missingParam);
            }
        }
    }
}
