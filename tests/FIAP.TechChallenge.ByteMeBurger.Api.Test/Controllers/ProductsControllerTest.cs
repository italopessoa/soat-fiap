using System.Collections.ObjectModel;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using FIAP.TechChallenge.ByteMeBurger.Api.Controllers;
using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Test.Controllers;

[TestSubject(typeof(ProductsController))]
public class ProductsControllerTest
{
    private readonly Mock<IProductService> _serviceMock;
    private readonly ProductsController _target;

    public ProductsControllerTest()
    {
        _serviceMock = new Mock<IProductService>();
        _target = new ProductsController(_serviceMock.Object, Mock.Of<ILogger<ProductsController>>());
    }

    [Fact]
    public async Task GetAll_Success()
    {
        // Arrange 
        var fixture = new Fixture();
        fixture.Customizations.Add(new ProductGenerator());
        var product = fixture.Create<Product>();
        _serviceMock.Setup(s => s.GetAll())
            .ReturnsAsync([product]);

        // Act
        var response = await _target.Get(null, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>()
                .Value.Should().BeOfType<ReadOnlyCollection<ProductDto>>()
                .And.BeEquivalentTo(new List<ProductDto>
                {
                    new(product)
                }.AsReadOnly());

            _serviceMock.Verify(s => s.GetAll(), Times.Once);
            _serviceMock.Verify(s => s.FindByCategory(It.IsAny<ProductCategory>()), Times.Never);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async Task GetAll_Empty()
    {
        // Arrange 
        _serviceMock.Setup(s => s.GetAll())
            .ReturnsAsync([]);

        // Act
        var response = await _target.Get(null, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>()
                .Value.As<ReadOnlyCollection<ProductDto>>()
                .Should().BeEmpty();

            _serviceMock.Verify(s => s.GetAll(), Times.Once);
            _serviceMock.Verify(s => s.FindByCategory(It.IsAny<ProductCategory>()), Times.Never);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async Task GetByCategory_Success()
    {
        // Arrange 
        var product = new Product(Guid.NewGuid(), "coca", "sem acucar", ProductCategory.Beverage, 2, []);
        _serviceMock.Setup(s => s.FindByCategory(It.IsAny<ProductCategory>()))
            .ReturnsAsync([product]);

        // Act
        var response = await _target.Get(ProductCategory.Beverage, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>()
                .Value.Should().BeOfType<ReadOnlyCollection<ProductDto>>()
                .And.BeEquivalentTo(new List<ProductDto>
                {
                    new(product)
                }.AsReadOnly());

            _serviceMock.Verify(s => s.GetAll(), Times.Never);
            _serviceMock.Verify(s => s.FindByCategory(It.Is<ProductCategory>(c => c == ProductCategory.Beverage)),
                Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async Task GetByCategory_Empty()
    {
        // Arrange 
        var product = new Product(Guid.NewGuid(), "coca", "sem acucar", ProductCategory.Beverage, 2, []);
        _serviceMock.Setup(s => s.FindByCategory(It.IsAny<ProductCategory>()))
            .ReturnsAsync([]);

        // Act
        var response = await _target.Get(ProductCategory.Beverage, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>()
                .Value.As<ReadOnlyCollection<ProductDto>>()
                .Should().BeEmpty();

            _serviceMock.Verify(s => s.GetAll(), Times.Never);
            _serviceMock.Verify(s => s.FindByCategory(It.Is<ProductCategory>(c => c == ProductCategory.Beverage)),
                Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async Task Delete_Success()
    {
        // Arrange 
        _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        // Act
        var response = await _target.Delete(Guid.NewGuid(), CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Should().BeOfType<OkResult>();

            _serviceMock.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async Task Delete_NoContent()
    {
        // Arrange 
        _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var response = await _target.Delete(Guid.NewGuid(), CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Should().BeOfType<NoContentResult>();

            _serviceMock.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async Task Delete_EmptyGuid()
    {
        // Arrange 
        // Act
        var response = await _target.Delete(Guid.Empty, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Should().BeOfType<BadRequestResult>();

            _serviceMock.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            _serviceMock.VerifyAll();
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_InvalidPrice_BadRequest(decimal price)
    {
        // Arrange 
        // Act
        var response = await _target.Create(new CreateProductCommandDto
        {
            Price = price
        }, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            response.Result.As<BadRequestObjectResult>().Value.Should().Be("Price cannot be zero ou negative.");


            _serviceMock.Verify(
                s => s.CreateAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<ProductCategory>(), It.IsAny<decimal>(), It.IsAny<string[]>()), Times.Never);
            _serviceMock.VerifyAll();
        }
    }

    [Theory]
    [InlineAutoData]
    public async Task Create_Success(CreateProductCommandDto newProductCommand)
    {
        // Arrange 
        _serviceMock.Setup(s => s.CreateAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<ProductCategory>(), It.IsAny<decimal>(), It.IsAny<string[]>()))
            .ReturnsAsync(newProductCommand.ToProduct());

        // Act
        var response = await _target.Create(newProductCommand, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<CreatedResult>();

            _serviceMock.Verify(
                s => s.CreateAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<ProductCategory>(), It.IsAny<decimal>(), It.IsAny<string[]>()), Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Theory]
    [InlineAutoData]
    public async Task Create_Error(CreateProductCommandDto newProductCommand)
    {
        // Arrange 
        _serviceMock.Setup(s => s.CreateAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<ProductCategory>(), It.IsAny<decimal>(), It.IsAny<string[]>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await _target.Create(newProductCommand, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            response.Result.As<BadRequestObjectResult>().Value.Should().Be("Unable to create the product.");

            _serviceMock.Verify(
                s => s.CreateAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<ProductCategory>(), It.IsAny<decimal>(), It.IsAny<string[]>()), Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Theory]
    [InlineAutoData]
    public async Task Update_Error(UpdateProductCommandDto updateProductCommandDto)
    {
        // Arrange 
        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<ProductCategory>(), It.IsAny<decimal>(), It.IsAny<string[]>()))
            .ReturnsAsync(false);

        // Act
        var response =
            await _target.Update(updateProductCommandDto.Id, updateProductCommandDto, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            response.Result.As<BadRequestObjectResult>().Value.Should().Be("Unable to update the product.");

            _serviceMock.Verify(
                s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<ProductCategory>(), It.IsAny<decimal>(), It.IsAny<string[]>()), Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async Task Update_InvalidId()
    {
        // Arrange 
        // Act
        var response =
            await _target.Update(Guid.Empty, new UpdateProductCommandDto(), CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<BadRequestObjectResult>();
            response.Result.As<BadRequestObjectResult>().Value.Should().Be("Invalid Id.");

            _serviceMock.Verify(
                s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<ProductCategory>(), It.IsAny<decimal>(), It.IsAny<string[]>()), Times.Never);
            _serviceMock.VerifyAll();
        }
    }

    [Theory]
    [InlineAutoData]
    public async Task Update_Success(UpdateProductCommandDto updateProductCommandDto)
    {
        // Arrange 
        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<ProductCategory>(), It.IsAny<decimal>(), It.IsAny<string[]>()))
            .ReturnsAsync(true);

        // Act
        var response =
            await _target.Update(updateProductCommandDto.Id, updateProductCommandDto, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<NoContentResult>();

            _serviceMock.Verify(
                s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<ProductCategory>(), It.IsAny<decimal>(), It.IsAny<string[]>()), Times.Once);
            _serviceMock.VerifyAll();
        }
    }
}

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