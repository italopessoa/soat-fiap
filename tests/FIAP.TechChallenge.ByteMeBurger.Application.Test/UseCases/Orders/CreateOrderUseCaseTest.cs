using AutoFixture.Xunit2;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.UseCases.Orders;

[TestSubject(typeof(CreateOrderUseCase))]
public class CreateOrderUseCaseTest
{
    private readonly Mock<ICustomerRepository> _customerRepository;
    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly Mock<IProductRepository> _productRepository;
    private readonly ICreateOrderUseCase _useCase;
    private readonly Cpf _validCpf = new("863.917.790-23");

    public CreateOrderUseCaseTest()
    {
        _customerRepository = new Mock<ICustomerRepository>();
        _productRepository = new Mock<IProductRepository>();
        _orderRepository = new Mock<IOrderRepository>();
        _useCase = new CreateOrderUseCase(_orderRepository.Object, _productRepository.Object,
            _customerRepository.Object);
    }

    [Theory]
    [InlineAutoData]
    public async Task Checkout_Success(
        List<(Guid productId, int quantity)> orderItems)
    {
        // Arrange
        var product = new Product("product", "description", ProductCategory.Drink, 10, []);
        var expectedCustomer = new Customer(Guid.NewGuid(), _validCpf, "customer", "customer@email.com");
        var expectedOrder = new Order(expectedCustomer);
        orderItems.ForEach(i => { expectedOrder.AddOrderItem(i.productId, product.Name, product.Price, i.quantity); });

        expectedOrder.Create();

        _orderRepository.Setup(r => r.CreateAsync(
                It.Is<Order>(o => o.Created != DateTime.MinValue
                                  && o.Status == OrderStatus.PaymentPending)))
            .ReturnsAsync(expectedOrder);

        _customerRepository.Setup(r => r.FindByCpfAsync(
                It.Is<string>(cpf => cpf == _validCpf.Value)))
            .ReturnsAsync(expectedCustomer);

        _productRepository.Setup(r => r.FindByIdAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(product);

        // Act
        var result = await _useCase.Execute(_validCpf, orderItems);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            _orderRepository.Verify(m => m.CreateAsync(
                It.Is<Order>(o => o.Created != DateTime.MinValue
                                  && o.Status == OrderStatus.PaymentPending)), Times.Once);

            _customerRepository.Verify(m => m.FindByCpfAsync(
                It.IsAny<string>()), Times.Once);

            _productRepository.Verify(m => m.FindByIdAsync(
                It.IsAny<Guid>()), Times.AtLeastOnce);
        }
    }

    [Theory]
    [InlineAutoData]
    public async Task Checkout_CustomerNotFound_Error(
        List<(Guid productId, int quantity)> orderItems)
    {
        // Arrange
        var expectedCustomer = new Customer(Guid.NewGuid(), _validCpf, "customer", "customer@email.com");
        var expectedOrder = new Order(expectedCustomer);
        orderItems.ForEach(i => { expectedOrder.AddOrderItem(i.productId, "productName", 1, i.quantity); });
        expectedOrder.Create();

        _customerRepository.Setup(r => r.FindByCpfAsync(
                It.Is<string>(cpf => cpf == _validCpf.Value)))
            .ReturnsAsync(default(Customer));

        // Act
        var func = async () => await _useCase.Execute(_validCpf, orderItems);

        // Assert
        using (new AssertionScope())
        {
            (await func.Should().ThrowExactlyAsync<EntityNotFoundException>())
                .And
                .Message
                .Should()
                .Be("Customer not found.");

            _orderRepository.Verify(m => m.CreateAsync(
                It.IsAny<Order>()), Times.Never);

            _customerRepository.Verify(m => m.FindByCpfAsync(
                It.IsAny<string>()), Times.Once);

            _productRepository.Verify(m => m.FindByIdAsync(
                It.IsAny<Guid>()), Times.Never);
        }
    }

    [Theory]
    [InlineAutoData]
    public async Task Checkout_ProductNotFound_Error(
        List<(Guid productId,int quantity)> orderItems)
    {
        // Arrange
        var expectedCustomer = new Customer(Guid.NewGuid(), _validCpf, "customer", "customer@email.com");
        var expectedOrder = new Order(expectedCustomer);
        orderItems.ForEach(i => { expectedOrder.AddOrderItem(i.productId, "productName", 2, i.quantity); });
        expectedOrder.Create();

        _customerRepository.Setup(r => r.FindByCpfAsync(
                It.Is<string>(cpf => cpf == _validCpf.Value)))
            .ReturnsAsync(expectedCustomer);

        _productRepository.Setup(r => r.FindByIdAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(default(Product));

        // Act
        var func = async () => await _useCase.Execute(_validCpf, orderItems);

        // Assert
        using (new AssertionScope())
        {
            (await func.Should().ThrowExactlyAsync<EntityNotFoundException>())
                .And
                .Message
                .Should()
                .Be($"Product '{orderItems.First().productId}' not found.");

            _orderRepository.Verify(m => m.CreateAsync(
                It.IsAny<Order>()), Times.Never);

            _customerRepository.Verify(m => m.FindByCpfAsync(
                It.IsAny<string>()), Times.Once);

            _productRepository.Verify(m => m.FindByIdAsync(
                It.IsAny<Guid>()), Times.Once);
        }
    }
}
