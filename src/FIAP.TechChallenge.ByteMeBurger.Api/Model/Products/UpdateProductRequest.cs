namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Products;

public class UpdateProductRequest : CreateProductRequest
{
    public Guid Id { get; set; }
}
