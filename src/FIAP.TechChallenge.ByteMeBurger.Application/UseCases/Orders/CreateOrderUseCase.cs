using Bmb.Domain.Core.Base;
using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Events;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;
using OrderCreated = Bmb.Domain.Core.Events.Integration.OrderCreated;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class CreateOrderUseCase : ICreateOrderUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderTrackingCodeService _orderTrackingCodeService;
    private readonly IDispatcher _analyticsPublisher;

    public CreateOrderUseCase(IProductRepository productRepository,
        IOrderTrackingCodeService orderTrackingCodeService, IDispatcher analyticsPublisher)
    {
        _productRepository = productRepository;
        _orderTrackingCodeService = orderTrackingCodeService;
        _analyticsPublisher = analyticsPublisher;
    }

    public async Task<Order> Execute(Customer? customer, List<SelectedProduct> selectedProducts)
    {
        var products = new Dictionary<Product, int>();
        foreach (var item in selectedProducts)
        {
            var product = await GetProduct(item.ProductId);
            if (product is null)
                throw new EntityNotFoundException($"Product '{item.ProductId}' not found.");

            products.Add(product, item.Quantity);
        }

        var trackingCode = await _orderTrackingCodeService.GetNextAsync();
        var order = new Order(customer, trackingCode, products);
        await _analyticsPublisher.PublishIntegrationAsync(MapToEvent(order));
        return order;
    }

    private async Task<Product?> GetProduct(Guid productId)
    {
        var product = await _productRepository.FindByIdAsync(productId);
        if (product == null)
            throw new EntityNotFoundException($"Product '{productId}' not found.");

        return product;
    }

    private OrderCreated MapToEvent(Order order)
    {
        var orderItemsReplica = order.OrderItems.Select(i =>
            new OrderCreated.OrderItemReplicaDto(i.ProductId, i.OrderId, i.ProductName, i.UnitPrice, i.Quantity));

        var customer = default(OrderCreated.CustomerReplicaDto);
        if (order.Customer is not null)
        {
            customer = new OrderCreated.CustomerReplicaDto(order.Customer.Id, order.Customer.Cpf, order.Customer.Name,
                order.Customer.Email);
        }
        return new OrderCreated(order.Id, customer
            , orderItemsReplica.ToList(), OrderStatus.PaymentPending,
            order.TrackingCode.Value, order.PaymentId, order.Total);
    }
}
