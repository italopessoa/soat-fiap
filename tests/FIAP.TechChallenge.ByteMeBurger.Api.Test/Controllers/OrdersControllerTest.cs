using AutoFixture;
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
using ILogger = Castle.Core.Logging.ILogger;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Test.Controllers;

[TestSubject(typeof(OrdersController))]
public class OrdersControllerTest
{
    private readonly Mock<IOrderService> _serviceMock;
    private readonly OrdersController _target;
    private readonly string cpf = "863.917.790-23";

    public OrdersControllerTest()
    {
        _serviceMock = new Mock<IOrderService>();
        _target = new OrdersController(_serviceMock.Object, Mock.Of<ILogger<OrdersController>>());
    }

    [Fact]
    public async void GetAll_Success()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "productA", "product description", ProductCategory.Beverage, 10, []);
        var orderId = Guid.NewGuid();
        var customer = new Customer(Guid.NewGuid(), cpf);
        var expectedOrder = new Order(orderId, customer);
        expectedOrder.AddOrderItem(product.Id, product.Name, product.Price, 10);

        var orders = new[]
        {
            expectedOrder
        };
        var expectedOrdersDto = new OrderDto[]
        {
            new(expectedOrder)
        };

        _serviceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(orders.ToList().AsReadOnly);

        // Act
        var response = await _target.Get(CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedOrdersDto);

            _serviceMock.Verify(o => o.GetAllAsync(), Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async void Create_WithCustomer_Success()
    {
        // Arrange
        var customer = new Customer(cpf, "name", "email@emil.com");
        var fixture = new Fixture();
        var orderId = Guid.NewGuid();
        var products = new Product[]
        {
            new(Guid.NewGuid(), "productA", "product description", ProductCategory.Beverage, 10, [])
        };
        var chosenProduct = products.First();

        var createOrderCommand = fixture.Build<CreateOrderCommandDto>()
            .With(c => c.Cpf, cpf)
            .Create();

        var expectedOrder = new Order(orderId, customer);
        expectedOrder.AddOrderItem(chosenProduct.Id, chosenProduct.Name, chosenProduct.Price, 10);
        expectedOrder.Checkout();

        _serviceMock.Setup(s => s.CreateAsync(It.IsAny<Cpf?>(),
                It.IsAny<List<(Guid productId, int quantity)>>()))
            .ReturnsAsync(expectedOrder);

        // Act
        var response = await _target.Create(createOrderCommand, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<CreatedResult>();
            var createdOrder = response.Result.As<CreatedResult>().Value.As<OrderDto>();

            // TODO after 1h or so I gave up trying to make this sh*t to assert it all in on line, I'll check it later 
            createdOrder.Id.Should().Be(expectedOrder.Id);

            createdOrder.Customer.Should().NotBeNull();
            createdOrder.Customer.Should()
                .BeEquivalentTo(customer, o => o.ComparingByMembers<Customer>()
                    .Excluding(c => c.Cpf));
            createdOrder.Customer.Cpf.Should().BeEquivalentTo(customer.Cpf);

            createdOrder.OrderItems.Should().BeEquivalentTo(expectedOrder.OrderItems,
                options => options.ComparingByMembers<OrderItem>());

            _serviceMock.Verify(
                s => s.CreateAsync(It.IsAny<Cpf?>(),
                    It.IsAny<List<(Guid productId, int quantity)>>()),
                Times.Once);

            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async void Create_NoCustomer_Success()
    {
        // Arrange
        var fixture = new Fixture();
        var orderId = Guid.NewGuid();
        var products = new Product[]
        {
            new(Guid.NewGuid(), "productA", "product description", ProductCategory.Beverage, 10, [])
        };
        var chosenProduct = products.First();

        var createOrderCommand = fixture.Build<CreateOrderCommandDto>()
            .With(c => c.Cpf, cpf)
            .Create();

        var expectedOrder = new Order(orderId, null);
        expectedOrder.AddOrderItem(chosenProduct.Id, chosenProduct.Name, chosenProduct.Price, 10);
        expectedOrder.Checkout();

        _serviceMock.Setup(s => s.CreateAsync(It.IsAny<Cpf?>(),
                It.IsAny<List<(Guid productId, int quantity)>>()))
            .ReturnsAsync(expectedOrder);

        // Act
        var response = await _target.Create(createOrderCommand, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<CreatedResult>();
            var createdOrder = response.Result.As<CreatedResult>().Value.As<OrderDto>();

            createdOrder.Id.Should().Be(expectedOrder.Id);
            createdOrder.Customer.Should().BeNull();
            createdOrder.OrderItems.Should().BeEquivalentTo(expectedOrder.OrderItems,
                options => options.ComparingByMembers<OrderItem>());

            _serviceMock.Verify(
                s => s.CreateAsync(It.IsAny<Cpf?>(),
                    It.IsAny<List<(Guid productId, int quantity)>>()),
                Times.Once);

            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async void Get_Detail_Success()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "productA", "product description", ProductCategory.Beverage, 10, []);
        var customerId = Guid.NewGuid();
        var expectedOrder = new Order(customerId, null);
        expectedOrder.AddOrderItem(product.Id, product.Name, product.Price, 10);

        var expectedOrderDto = new OrderDto(expectedOrder);

        _serviceMock.Setup(s => s.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedOrder);

        // Act
        var response = await _target.Get(expectedOrder.Id, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedOrderDto);

            _serviceMock.Verify(o => o.GetAsync(It.IsAny<Guid>()), Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async void Get_Detail_NotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(default(Order));

        // Act
        var response = await _target.Get(Guid.NewGuid(), CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<NotFoundResult>();

            _serviceMock.Verify(o => o.GetAsync(It.IsAny<Guid>()), Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async void Get_Detail_InvalidId_BadRequest()
    {
        // Arrange
        // Act
        var response = await _target.Get(Guid.Empty, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<BadRequestObjectResult>();

            _serviceMock.Verify(o => o.GetAsync(It.IsAny<Guid>()), Times.Never);
            _serviceMock.VerifyAll();
        }
    }
}