using System.ComponentModel.DataAnnotations;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Customers;

/// <summary>
/// Create new Customer Request
/// </summary>
public record CreateCustomerRequest
{
    /// <summary>
    /// Customer name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Customer e-mail
    /// </summary>
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// Customer CPF
    /// </summary>
    [Required]
    [MaxLength(14)]
    public string Cpf { get; set; }
}
