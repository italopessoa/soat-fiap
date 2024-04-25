namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class UpdateProductCommandDto : CreateProductCommandDto
{
    public Guid Id { get; set; }
}