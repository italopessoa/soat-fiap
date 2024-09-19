using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Persistence.Dto;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence;

public static class Presenter
{
    public static Product? FromDtoToEntity(this ProductDto dto)
    {
        var images = dto.Images?.Split("|") ?? Array.Empty<string>();
        return new Product(dto.Id, dto.Name, dto.Description, (ProductCategory)dto.Category, dto.Price, images);
    }

    public static Customer FromDtoToEntity(this CustomerDto? dto)
    {
        if (dto is null) return null;

        var customer = new Customer(dto.Id, dto.Cpf);
        if (dto.Email is not null) customer.ChangeEmail(dto.Email);
        if (dto.Name is not null) customer.ChangeName(dto.Name);
        return customer;
    }

    public static ProductDto FromEntityToDto(this Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Category = (int)product.Category,
        Price = product.Price,
        Created = product.Created,
        Updated = product.Updated,
        Images = string.Join("|", product.Images)
    };

    public static CustomerDto FromEntityToDto(this Customer? customer)
    {
        if (customer is null) return null;
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Cpf = customer.Cpf
        };
    }
}
