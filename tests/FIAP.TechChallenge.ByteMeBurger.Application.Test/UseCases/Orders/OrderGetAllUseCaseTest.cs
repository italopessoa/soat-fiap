using System.Collections.ObjectModel;
using AutoFixture;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.UseCases.Orders;

[TestSubject(typeof(OrderGetAllUseCase))]
public class OrderGetAllUseCaseTest
{
    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly IOrderGetAllUseCase _useCase;

    public OrderGetAllUseCaseTest()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _useCase = new OrderGetAllUseCase(_orderRepository.Object);
    }

    [Fact]
    public async Task GetAll_Success()
    {
        // Arrange 
        var expectedOrders = new Fixture().CreateMany<Order>().ToList();
        _orderRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(expectedOrders.AsReadOnly);

        // Act
        var result = await _useCase.Execute();

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(expectedOrders);
            _orderRepository.Verify(m => m.GetAllAsync(), Times.Once);
        }
    }

    [Fact]
    public async Task GetAll_Empty()
    {
        // Arrange 
        _orderRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync((ReadOnlyCollection<Order>)default!);

        // Act
        var result = await _useCase.Execute();

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _orderRepository.Verify(m => m.GetAllAsync(), Times.Once);
        }
    }
}