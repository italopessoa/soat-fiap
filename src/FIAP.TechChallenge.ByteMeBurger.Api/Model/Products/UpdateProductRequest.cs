namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Products;

/// <summary>
/// Update product Request
/// </summary>
public class UpdateProductRequest : CreateProductRequest
{
    /// <summary>
    /// Product Id
    /// </summary>
    public Guid Id { get; set; }
}
