using Amazon.CognitoIdentityProvider;
using FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway.Factory;

namespace FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway;

public class CognitoClientFactory : ICognitoClientFactory
{
    public IAmazonCognitoIdentityProvider CreateClient()
    {
        return new AmazonCognitoIdentityProviderClient();
    }
}
