using AutoFixture;
using AutoFixture.Kernel;
using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Test.Common;

public class ProductGenerator : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        var type = request as Type;
        if (type != typeof(Product))
            return new NoSpecimen();

        return new Product(context.Create<string>(), context.Create<string>(), context.Create<ProductCategory>(),
            context.Create<decimal>(), context.CreateMany<string>().ToList().AsReadOnly());
    }
}
