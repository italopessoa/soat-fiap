using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;

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

    public CustomerDto()
    {

    }

    public CustomerDto(Guid customerId, Cpf cpf, string customerName, string customerEmailCom)
    {
        Id = customerId;
        Cpf = cpf;
        Name = customerName;
        Email = customerEmailCom;
    }
}
