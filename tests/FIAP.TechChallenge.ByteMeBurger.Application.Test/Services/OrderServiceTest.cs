using System.Collections.ObjectModel;
using AutoFixture;
using AutoFixture.Xunit2;
using FIAP.TechChallenge.ByteMeBurger.Application.Services;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.Services;

[TestSubject(typeof(OrderService))]
public class OrderServiceTest
{
    private readonly Mock<IOrderRepository> _mockRepository;

    private readonly OrderService _target;

    public OrderServiceTest()
    {
        _mockRepository = new Mock<IOrderRepository>();
        _target = new OrderService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAll_Success()
    {
        // Arrange 
        var expectedOrders = new Fixture().CreateMany<Order>().ToList();
        _mockRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(expectedOrders.AsReadOnly);

        // Act
        var result = await _target.GetAllAsync();

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(expectedOrders);
            _mockRepository.Verify(m => m.GetAllAsync(), Times.Once);
        }
    }

    [Fact]
    public async Task GetAll_Empty()
    {
        // Arrange 
        _mockRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync((ReadOnlyCollection<Order>)default!);

        // Act
        var result = await _target.GetAllAsync();

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _mockRepository.Verify(m => m.GetAllAsync(), Times.Once);
        }
    }

    [Theory]
    [InlineAutoData]
    public async Task Create_Success(
        List<(Guid productId, string productName, int quantity, decimal unitPrice)> orderItems)
    {
        // Arrange 
        var customerId = Guid.NewGuid().ToString();
        var expectedOrder = new Order(customerId);
        orderItems.ForEach(i => { expectedOrder.AddOrderItem(i.productId, i.productName, i.unitPrice, i.quantity); });

        expectedOrder.Checkout();

        _mockRepository.Setup(r => r.CreateAsync(
                It.Is<Order>(o => o.Created != DateTime.MinValue
                                  && o.Status == OrderStatus.PaymentPending)))
            .ReturnsAsync(expectedOrder);

        // Act
        var result = await _target.CreateAsync(customerId, orderItems);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            _mockRepository.Verify(m => m.CreateAsync(
                It.Is<Order>(o => o.Created != DateTime.MinValue
                                  && o.Status == OrderStatus.PaymentPending)), Times.Once);
        }
    }

    [Fact]
    public async Task Get_Success()
    {
        // Arrange 
        var expectedOrder = new Fixture().Create<Order>();

        _mockRepository.Setup(r => r.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedOrder);

        // Act
        var result = await _target.GetAsync(expectedOrder.Id);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().Be(expectedOrder);
            _mockRepository.Verify(m => m.GetAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
    
    [Fact]
    public async Task Get_NotFound()
    {
        // Arrange 

        _mockRepository.Setup(r => r.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await _target.GetAsync(Guid.NewGuid());

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeNull();
            _mockRepository.Verify(m => m.GetAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
    
    [Fact]
    public async Task Get_InvalidId_Null()
    {
        // Arrange 
        _mockRepository.Setup(r => r.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await _target.GetAsync(Guid.Empty);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeNull();
            _mockRepository.Verify(m => m.GetAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}