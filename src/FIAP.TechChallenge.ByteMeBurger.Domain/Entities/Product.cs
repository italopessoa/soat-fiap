using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public class Product : Entity<Guid>
{
    public string Name { get; private set; }

    public string Description { get; private set; }

    public ProductCategory Category { get; private set; }

    public decimal Price { get; private set; }

    public IReadOnlyList<string> Images { get; private set; }

    public DateTime CreationDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public Product()
    {
    }

    public Product(string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images) : this(Guid.NewGuid(), name, description, category, price, images)
    {
    }

    public Product(Guid id, string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images)
        : base(id)
    {
        Validate(name, description, price);

        Name = name.ToUpper();
        Description = description.ToUpper();
        Category = category;
        Price = price;
        Images = images;
    }

    public void Create()
    {
        CreationDate = DateTime.UtcNow;
    }

    public void SetImages(string[] images)
    {
        if (images?.Length > 0)
            Images = images.AsReadOnly();
    }

    public void Update(Product oldProduct)
    {
        Name = oldProduct.Name;
        Description = oldProduct.Description;
        Category = oldProduct.Category;
        Price = oldProduct.Price;
        Images = oldProduct.Images;
        LastUpdate = DateTime.UtcNow;
    }

    private static void Validate(string name, string description, decimal price)
    {
        AssertionConcern.AssertArgumentNotEmpty(name, nameof(name));
        AssertionConcern.AssertArgumentNotEmpty(description,nameof(description));
        AssertionConcern. AssertArgumentNotZeroOrNegative(price, nameof(price));
    }
}