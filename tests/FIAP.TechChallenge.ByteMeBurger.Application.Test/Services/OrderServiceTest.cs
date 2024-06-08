using AutoFixture;
using AutoFixture.Xunit2;
using FIAP.TechChallenge.ByteMeBurger.Application.Services;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.Services;

[TestSubject(typeof(OrderService))]
public class OrderServiceTest
{
    private readonly Mock<ICreateOrderUseCase> _mockCreateOrderUseCase;
    private readonly Mock<IGetOrderDetailsUseCase> _mockGetOrderDetailsUseCase;
    private readonly Mock<IOrderGetAllUseCase> _mockOrderGetAllUseCase;
    private readonly Mock<ICheckoutOrderUseCase> _mockCheckoutOrderUseCase;

    private readonly OrderService _target;
    private readonly Cpf _validCpf = new("863.917.790-23");

    public OrderServiceTest()
    {
        _mockCreateOrderUseCase = new Mock<ICreateOrderUseCase>();
        _mockGetOrderDetailsUseCase = new Mock<IGetOrderDetailsUseCase>();
        _mockOrderGetAllUseCase = new Mock<IOrderGetAllUseCase>();
        _mockCheckoutOrderUseCase = new Mock<ICheckoutOrderUseCase>();
        _target = new OrderService(_mockCreateOrderUseCase.Object, _mockGetOrderDetailsUseCase.Object,
            _mockOrderGetAllUseCase.Object, _mockCheckoutOrderUseCase.Object);
    }

    [Fact]
    public async Task GetAll_Success()
    {
        // Arrange
        var expectedOrders = new Fixture().CreateMany<Order>().ToList();
        _mockOrderGetAllUseCase.Setup(r => r.Execute())
            .ReturnsAsync(expectedOrders.AsReadOnly);

        // Act
        var result = await _target.GetAllAsync();

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(expectedOrders);
            _mockOrderGetAllUseCase.Verify(m => m.Execute(), Times.Once);
        }
    }

    [Fact]
    public async Task GetAll_Empty()
    {
        // Arrange
        _mockOrderGetAllUseCase.Setup(r => r.Execute())
            .ReturnsAsync(Array.Empty<Order>().AsReadOnly);

        // Act
        var result = await _target.GetAllAsync();

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _mockOrderGetAllUseCase.Verify(m => m.Execute(), Times.Once);
        }
    }

    [Theory]
    [InlineAutoData]
    public async Task Create_Success(
        List<SelectedProduct> selectedProducts)
    {
        // Arrange
        var expectedCustomer = new Customer(Guid.NewGuid(), _validCpf, "customer", "customer@email.com");
        var expectedOrder = new Order(expectedCustomer);
        selectedProducts.ForEach(i => { expectedOrder.AddOrderItem(i.ProductId, "product name", 1, i.Quantity); });

        expectedOrder.Create();
        _mockCreateOrderUseCase.Setup(s => s.Execute(It.IsAny<Cpf?>(),
                It.IsAny<List<SelectedProduct>>()))
            .ReturnsAsync(expectedOrder);

        // Act
        var result = await _target.CreateAsync(_validCpf, selectedProducts);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            _mockCreateOrderUseCase.Verify(s => s.Execute(It.IsAny<Cpf?>(),
                It.IsAny<List<SelectedProduct>>()), Times.Once);
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
            result.Should().Be(expectedOrder);
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
    public async Task Checkout_Success()
    {
        // Arrange
        _mockCheckoutOrderUseCase.Setup(r => r.Execute(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _target.CheckoutAsync(Guid.NewGuid());

        // Assert
        using (new AssertionScope())
        {
            _mockCheckoutOrderUseCase.VerifyAll();
        }
    }
}
