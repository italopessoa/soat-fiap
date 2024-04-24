using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class CustomerDto
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public CustomerDto()
    {
    }

    public CustomerDto(Customer customer)
    {
        Id = customer.Id;
        Name = customer.Name;
        Email = customer.Email;
    }
}