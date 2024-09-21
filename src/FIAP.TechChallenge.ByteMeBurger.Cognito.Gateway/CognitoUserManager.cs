using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway.Factory;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using Microsoft.Extensions.Options;

namespace FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway;

public class CognitoUserManager : ICustomerRepository
{
    private readonly IAmazonCognitoIdentityProvider _cognitoClient;
    private readonly string _userPoolId;
    private readonly string _clientId;

    public CognitoUserManager(ICognitoClientFactory cognitoClientFactory, IOptions<CognitoSettings> settings)
    {
        _cognitoClient = cognitoClientFactory.CreateClient();
        _userPoolId = settings.Value.UserPoolId;
        _clientId = settings.Value.UserPoolClientId;
    }

    public async Task<Customer?> FindByCpfAsync(string cpf)
    {
        try
        {
            var response = await _cognitoClient.AdminGetUserAsync(new AdminGetUserRequest
            {
                UserPoolId = _userPoolId,
                Username = cpf
            });

            var email = response.UserAttributes.First(attr => attr.Name == "email").Value;
            var name = response.UserAttributes.First(attr => attr.Name == "name").Value;
            var sub = response.UserAttributes.First(attr => attr.Name == "sub").Value;
            var customer = new Customer(Guid.Parse(sub), cpf, name, email);

            return customer;
        }
        catch (UserNotFoundException)
        {
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user: {ex.Message}");
            throw;
        }
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        await RegisterUserAndAssignToGroupAsync(customer);
        return customer;
    }

    private async Task<Guid> RegisterUserAndAssignToGroupAsync(Customer customer)
    {
        try
        {
            // Step 1: Sign up the user
            var signUpResponse = await _cognitoClient.SignUpAsync(new SignUpRequest
            {
                ClientId = _clientId,
                Username = customer.Cpf,
                Password = GenerateRandomPassword(10),
                UserAttributes =
                [
                    new AttributeType { Name = "email", Value = customer.Email },
                    new AttributeType { Name = "name", Value = customer.Name }
                ],
            });

            // var addToGroupRequest = new AdminAddUserToGroupRequest
            // {
            //     UserPoolId = _userPoolId,
            //     Username = customer.Cpf,
            //     GroupName = groupName
            // };
            //
            // await _cognitoClient.AdminAddUserToGroupAsync(addToGroupRequest);

            return Guid.Parse(signUpResponse.UserSub);
        }
        catch (UsernameExistsException ex)
        {
            Console.WriteLine($"Error registering user: {ex.Message}");
            throw new DomainException("There's already a customer using the provided CPF value.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error registering user: {ex.Message}");
            throw;
        }
    }

    private static string GenerateRandomPassword(int length)
    {
        const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
        const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digitChars = "1234567890";
        const string specialChars = "!@#$%^&*()";
        const string allChars = lowerChars + upperChars + digitChars + specialChars;

        var random = new Random();
        var password = new char[length];

        // Ensure the password contains at least one of each required character type
        password[0] = lowerChars[random.Next(lowerChars.Length)];
        password[1] = upperChars[random.Next(upperChars.Length)];
        password[2] = digitChars[random.Next(digitChars.Length)];
        password[3] = specialChars[random.Next(specialChars.Length)];

        // Fill the rest of the password with random characters
        for (int i = 4; i < length; i++)
        {
            password[i] = allChars[random.Next(allChars.Length)];
        }

        // Shuffle the password to ensure randomness
        return new string(password.OrderBy(x => random.Next()).ToArray());
    }
}
