using Bmb.Domain.Core.Events;
using Bmb.Domain.Core.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using Xunit.Gherkin.Quick;
using OrderCreated = Bmb.Domain.Core.Events.Integration.OrderCreated;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.UseCases.Orders.Gherkin;

[FeatureFile("./UseCases/Orders/Gherkin/CreateOrder.feature")]
public class CreateOrderSteps : Feature
{
// Given Selected product exists
// And Tracking code is created
// When UseCase is called
// Then it should create the order
    private readonly CreateOrderUseCase _useCase;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IOrderTrackingCodeService> _mockOrderTrackingCodeService;
    private readonly Mock<IDispatcher> _mockDispatcher;
    private Order _order;
    private Product _product;

    public CreateOrderSteps()
    {
        _mockDispatcher = new Mock<IDispatcher>();
        _mockProductRepository = new Mock<IProductRepository>();
        _mockOrderTrackingCodeService = new Mock<IOrderTrackingCodeService>();

        _useCase = new CreateOrderUseCase(_mockProductRepository.Object, _mockOrderTrackingCodeService.Object,
            _mockDispatcher.Object);
    }

    [Given("Selected product exists")]
    public void SetupExistingProduct()
    {
        _product = new Product(Guid.NewGuid(), "product", "description", ProductCategory.Drink, 10, []);
        _mockProductRepository.Setup(p => p.FindByIdAsync(_product.Id))
            .ReturnsAsync(_product)
            .Verifiable();
    }

    [And("Tracking code is created")]
    public void SetupTrackingCode()
    {
        _mockOrderTrackingCodeService.Setup(s => s.GetNextAsync())
            .ReturnsAsync(new OrderTrackingCode("code"))
            .Verifiable();
    }

    [When("UseCase is called")]
    public async Task WhenUseCaseIsCalled()
    {
        _order = await _useCase.Execute(null, new List<SelectedProduct>
        {
            new(_product.Id, 1)
        });
    }

    [Then("it should create the order")]
    public void ThenItShouldCreateTheOrder()
    {
        _order.Should().NotBeNull();
        _order.TrackingCode.Should().Be(new OrderTrackingCode("code"));
        _order.OrderItems[0].ProductId.Should().Be(_product.Id);
        _mockProductRepository.VerifyAll();
    }

    [And("it should publish integration event")]
    public void ThenItShouldPublishTheEvent()
    {
        _mockDispatcher.Verify(d => d.PublishIntegrationAsync(It.IsAny<OrderCreated>(), default), Times.Once);
    }
}
