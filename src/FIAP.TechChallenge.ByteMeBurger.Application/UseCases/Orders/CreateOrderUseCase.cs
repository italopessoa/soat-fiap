using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class CreateOrderUseCase : ICreateOrderUseCase
{
    private readonly IOrderRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;

    public CreateOrderUseCase(IOrderRepository repository, IProductRepository productRepository,
        ICustomerRepository customerRepository)
    {
        _repository = repository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Order> Execute(Cpf? customerCpf, List<SelectedProduct> selectedProducts)
    {
        var customer = default(Customer);
        if (customerCpf is not null)
        {
            customer = await _customerRepository.FindByCpfAsync(customerCpf);
            if (customer == null)
                throw new EntityNotFoundException("Customer not found.");
        }

        var products = new Dictionary<Product, int>();
        foreach (var item in selectedProducts)
        {
            var product = await GetProduct(item.ProductId);
            if (product is null)
                throw new EntityNotFoundException($"Product '{item.ProductId}' not found.");

            products.Add(product, item.Quantity);
        }

        var order = new Order(customer,"code", products);
        DomainEventTrigger.RaiseOrderCreated(order);
        return await _repository.CreateAsync(order);
    }

    private async Task<Product?> GetProduct(Guid productId)
    {
        var product = await _productRepository.FindByIdAsync(productId);
        if (product == null)
            throw new EntityNotFoundException($"Product '{productId}' not found.");

        return product;
    }
}
