using Amazon.CognitoIdentityProvider;

namespace FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway.Factory;

public interface ICognitoClientFactory
{
    IAmazonCognitoIdentityProvider CreateClient();
}
