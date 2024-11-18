using AutoFixture;
using AutoFixture.Xunit2;
using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;
using Bmb.Orders.Domain.Contracts;
using Bmb.Orders.Domain.Entities;
using Bmb.Orders.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers.Test;

[TestSubject(typeof(OrderService))]
public class OrderServiceTest
{
    private readonly Mock<ICreateOrderUseCase> _mockCreateOrderUseCase;
    private readonly Mock<IGetOrderDetailsUseCase> _mockGetOrderDetailsUseCase;
    private readonly Mock<IOrderGetAllUseCase> _mockOrderGetAllUseCase;
    private readonly Mock<IUseCase<UpdateOrderStatusRequest, bool>> _mockUpdateOrderStatusUseCase;
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IUpdateOrderPaymentUseCase> _mockUpdateOrderPaymentUseCase;


    private readonly OrderService _target;
    private readonly Cpf _validCpf = new("863.917.790-23");

    public OrderServiceTest()
    {
        _mockCreateOrderUseCase = new Mock<ICreateOrderUseCase>();
        _mockGetOrderDetailsUseCase = new Mock<IGetOrderDetailsUseCase>();
        _mockOrderGetAllUseCase = new Mock<IOrderGetAllUseCase>();
        _mockUpdateOrderStatusUseCase = new Mock<IUseCase<UpdateOrderStatusRequest, bool>>();
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockUpdateOrderPaymentUseCase = new Mock<IUpdateOrderPaymentUseCase>();

        _target = new OrderService(_mockCreateOrderUseCase.Object, _mockGetOrderDetailsUseCase.Object,
            _mockOrderGetAllUseCase.Object, _mockOrderRepository.Object, _mockUpdateOrderStatusUseCase.Object,
            _mockUpdateOrderPaymentUseCase.Object);
    }

    [Fact]
    public async Task GetAll_Success()
    {
        // Arrange
        var expectedOrders = new Fixture().CreateMany<Order>().ToList();
        _mockOrderGetAllUseCase.Setup(r => r.Execute(true))
            .ReturnsAsync(expectedOrders.AsReadOnly);

        // Act
        var result = await _target.GetAllAsync(true);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(expectedOrders.FromDomainToDto());
            _mockOrderGetAllUseCase.Verify(m => m.Execute(true), Times.Once);
        }
    }

    [Fact]
    public async Task GetAll_Empty()
    {
        // Arrange
        _mockOrderGetAllUseCase.Setup(r => r.Execute(true))
            .ReturnsAsync(Array.Empty<Order>().AsReadOnly);

        // Act
        var result = await _target.GetAllAsync(true);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _mockOrderGetAllUseCase.Verify(m => m.Execute(true), Times.Once);
        }
    }

    [Theory]
    [InlineAutoData]
    public async Task Create_Success(List<SelectedProduct> selectedProducts)
    {
        // Arrange
        var customerDto = new CustomerDto(Guid.NewGuid(), _validCpf, "customer", "customer@email.com");
        var expectedOrder = new Order(customerDto.ToDomain());
        expectedOrder.SetTrackingCode("trackingCode");
        selectedProducts.ForEach(i => { expectedOrder.AddOrderItem(i.ProductId, "product name", 1, i.Quantity); });

        _mockCreateOrderUseCase.Setup(s => s.Execute(It.IsAny<Customer?>(),
                It.IsAny<List<SelectedProduct>>()))
            .ReturnsAsync(expectedOrder);

        // Act
        var result = await _target.CreateAsync(customerDto, selectedProducts);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            _mockCreateOrderUseCase.Verify(s => s.Execute(It.IsAny<Customer?>(),
                It.IsAny<List<SelectedProduct>>()), Times.Once);

            result.Should().NotBeNull();
            _mockOrderRepository.Verify(m => m.CreateAsync(
                It.Is<Order>(o => o.Created != DateTime.MinValue
                                  && o.Status == OrderStatus.PaymentPending)), Times.Once);
        }
    }

    [Fact]
    public async Task Get_Success()
    {
        // Arrange
        var expectedOrder = new Fixture().Create<Order>();

        _mockGetOrderDetailsUseCase.Setup(r => r.Execute(It.IsAny<Guid>()))
            .ReturnsAsync(expectedOrder);

        // Act
        var result = await _target.GetAsync(expectedOrder.Id);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedOrder.FromEntityToDto());
            _mockGetOrderDetailsUseCase.Verify(m => m.Execute(It.IsAny<Guid>()), Times.Once);
        }
    }

    [Fact]
    public async Task Get_NotFound()
    {
        // Arrange
        _mockGetOrderDetailsUseCase.Setup(r => r.Execute(It.IsAny<Guid>()))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await _target.GetAsync(Guid.NewGuid());

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeNull();
            _mockGetOrderDetailsUseCase.Verify(m => m.Execute(It.IsAny<Guid>()), Times.Once);
        }
    }

    [Fact]
    public async Task UpdateStatusAsync_Success()
    {
        // Arrange
        _mockUpdateOrderStatusUseCase.Setup(r => r.ExecuteAsync(It.IsAny<UpdateOrderStatusRequest>()))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var updated = await _target.UpdateStatusAsync(Guid.NewGuid(), OrderStatus.Ready);

        // Assert
        using (new AssertionScope())
        {
            updated.Should().BeTrue();
        }
    }

    [Fact]
    public async Task UpdateOrderPaymentAsync_ShouldThrowException_WhenUseCaseThrowsException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var paymentId = new PaymentId(Guid.NewGuid());
        var exception = new Exception("Unexpected error");
        _mockUpdateOrderPaymentUseCase.Setup(x => x.Execute(orderId, paymentId)).ThrowsAsync(exception);

        // Act
        Func<Task> act = async () => await _target.UpdateOrderPaymentAsync(orderId, paymentId);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Unexpected error");
        _mockUpdateOrderPaymentUseCase.Verify(x => x.Execute(orderId, paymentId), Times.Once);
    }
}
