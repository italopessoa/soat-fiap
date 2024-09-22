using System.Security.Cryptography;
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
        try
        {
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

            customer.Id = Guid.Parse(signUpResponse.User.Attributes.First(a=>a.Name is "sub").Value);
            return customer;
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
        using var rng = RandomNumberGenerator.Create();
        var characterSets = new[]
        {
            "abcdefghijklmnopqrstuvwxyz",
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
            "1234567890",
            "!@#$%^&*()"
        };
        var allChars = string.Concat(characterSets);
        var passwordChars = new char[length];
        // Ensure the password contains at least one character from each character set
        for (int i = 0; i < characterSets.Length && i < length; i++)
        {
            passwordChars[i] = GetRandomChar(characterSets[i], rng);
        }
        // Fill the rest of the password with random characters
        for (int i = characterSets.Length; i < length; i++)
        {
            passwordChars[i] = GetRandomChar(allChars, rng);
        }
        // Shuffle the password to ensure randomness
        passwordChars = passwordChars.OrderBy(_ => GetRandomInt(rng, int.MaxValue)).ToArray();
        return new string(passwordChars);
    }
    private static char GetRandomChar(string chars, RandomNumberGenerator rng)
    {
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        var index = BitConverter.ToUInt32(bytes, 0) % chars.Length;
        return chars[(int)index];
    }
    private static int GetRandomInt(RandomNumberGenerator rng, int max)
    {
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        return (int)(BitConverter.ToUInt32(bytes, 0) % max);
    }
}
