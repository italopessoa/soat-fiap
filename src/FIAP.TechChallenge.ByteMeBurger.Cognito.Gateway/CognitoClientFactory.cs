using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Runtime;
using FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway.Factory;
using Microsoft.Extensions.Options;

namespace FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway;

public class CognitoClientFactory(IOptions<CognitoSettings> settings) : ICognitoClientFactory
{
    public IAmazonCognitoIdentityProvider CreateClient()
    {
        return new AmazonCognitoIdentityProviderClient(
            new BasicAWSCredentials(settings.Value.ClientId, settings.Value.ClientSecret),
            RegionEndpoint.GetBySystemName(settings.Value.Region));
    }
}
