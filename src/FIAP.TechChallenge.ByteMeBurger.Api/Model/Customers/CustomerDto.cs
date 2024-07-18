namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Customers;

/// <summary>
/// Customer detail
/// </summary>
public class CustomerDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// Customer CPF
    /// </summary>
    public string Cpf { get; set; }

    /// <summary>
    /// Customer name
    /// </summary>
    public string? Name { get; set; }


    /// <summary>
    /// Customer E-mail
    /// </summary>
    public string? Email { get; set; }
}
