using FIAP.TechChallenge.ByteMeBurger.Domain.Base;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

public class Product : ValueObject
{
    public string Name { get; private set; }

    public string Description { get; private set; }

    public ProductCategory Category { get; private set; }

    public decimal Price { get; private set; }

    public IReadOnlyList<string> Images { get; private set; }

    public Product(string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentOutOfRangeException.ThrowIfNegative(price);

        Name = name.ToUpper();
        Description = description.ToUpper();
        Category = category;
        Price = price;
        Images = images;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Description;
        yield return Category;
        yield return Price;
        yield return Images.GetEnumerator();
    }
}