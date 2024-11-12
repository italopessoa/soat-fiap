using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Interfaces;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway.Factory;
using Bmb.Domain.Core.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway;

public class CognitoUserManager(
    ICognitoClientFactory cognitoClientFactory,
    ILogger<CognitoUserManager> logger,
    IOptions<CognitoSettings> settings)
    : ICustomerRepository
{
    private readonly IAmazonCognitoIdentityProvider _cognitoClient = cognitoClientFactory.CreateClient();
    private readonly string _userPoolId = settings.Value.UserPoolId;

    public async Task<Customer?> FindByCpfAsync(string cpf)
    {
        try
        {
            logger.LogInformation("Fetching user with CPF {cpf}", cpf);
            var response = await _cognitoClient.AdminGetUserAsync(new AdminGetUserRequest
            {
                UserPoolId = _userPoolId,
                Username = cpf,
            });

            var attributes = response.UserAttributes;

            var email = attributes.First(attr => attr.Name == "email").Value;
            var name = attributes.First(attr => attr.Name == "name").Value;
            var sub = attributes.First(attr => attr.Name == "sub").Value;
            return new Customer(Guid.Parse(sub), cpf, name, email);
        }
        catch (UserNotFoundException)
        {
            logger.LogWarning("Customer not found CPF {cpf}", cpf);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching user");
            throw;
        }
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        try
        {
            logger.LogInformation("Trying to create new customer");
            var signUpResponse = await _cognitoClient.AdminCreateUserAsync(new AdminCreateUserRequest()
            {
                Username = customer.Cpf,
                UserPoolId = _userPoolId,
                UserAttributes =
                {
                    new AttributeType { Name = "email", Value = customer.Email },
                    new AttributeType { Name = "name", Value = customer.Name }
                }
            });

            await _cognitoClient.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
            {
                UserPoolId = _userPoolId,
                Username = customer.Cpf,
                GroupName = "customer"
            });

            logger.LogInformation("Customer successfully created.");
            customer.Id = Guid.Parse(signUpResponse.User.Attributes.First(a => a.Name is "sub").Value);
            return customer;
        }
        catch (UsernameExistsException ex)
        {
            logger.LogWarning(ex, "There's already a customer using the provided CPF value");
            throw new DomainException("There's already a customer using the provided CPF value.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error registering user");
            throw;
        }
    }

    public async Task<Customer?> FindByIdAsync(Guid id)
    {
        try
        {
            logger.LogInformation("Fetching user with Id {CustomerId}", id);
            var response = await _cognitoClient.ListUsersAsync(new ListUsersRequest()
            {
                Filter = "sub=\"" + id + "\"",
                UserPoolId = _userPoolId,
            });

            if (response.Users.Count > 0)
            {
                var attributes = response.Users[0].Attributes;

                var email = attributes.First(attr => attr.Name == "email").Value;
                var name = attributes.First(attr => attr.Name == "name").Value;
                var sub = attributes.First(attr => attr.Name == "sub").Value;
                var cpf = response.Users[0].Username;
                return new Customer(Guid.Parse(sub), cpf, name, email);
            }

            return default;
        }
        catch (UserNotFoundException)
        {
            logger.LogWarning("User not found.");
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching user");
            throw;
        }
    }
}
