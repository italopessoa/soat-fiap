using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class CreateOrderUseCase : ICreateOrderUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderTrackingCodeService _orderTrackingCodeService;

    public CreateOrderUseCase(IProductRepository productRepository,
        IOrderTrackingCodeService orderTrackingCodeService)
    {
        _productRepository = productRepository;
        _orderTrackingCodeService = orderTrackingCodeService;
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
        DomainEventTrigger.RaiseOrderCreated(order);
        return order;
    }

    private async Task<Product?> GetProduct(Guid productId)
    {
        var product = await _productRepository.FindByIdAsync(productId);
        if (product == null)
            throw new EntityNotFoundException($"Product '{productId}' not found.");

        return product;
    }
}
