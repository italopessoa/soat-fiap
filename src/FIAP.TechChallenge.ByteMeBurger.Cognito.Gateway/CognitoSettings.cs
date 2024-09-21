namespace FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway;

/// <summary>
/// Cognito User Pool settings
/// </summary>
public class CognitoSettings
{
    /// <summary>
    /// User Pool Id
    /// </summary>
    public string UserPoolId { get; set; } = string.Empty;


    /// <summary>
    /// Client Id
    /// </summary>
    public string UserPoolClientId { get; set; } = string.Empty;

    /// <summary>
    /// Enabled
    /// </summary>
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// AWS Region
    /// </summary>
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// AWS Secret Id
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// AWS Client Id
    /// </summary>
    public string ClientId { get; set; } = string.Empty;
}
