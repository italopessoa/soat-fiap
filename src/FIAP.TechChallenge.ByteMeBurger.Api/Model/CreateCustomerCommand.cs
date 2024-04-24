using System.ComponentModel.DataAnnotations;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class CreateCustomerCommand
{
    public string? Name { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(14)]
    public string? Cpf { get; set; }
}