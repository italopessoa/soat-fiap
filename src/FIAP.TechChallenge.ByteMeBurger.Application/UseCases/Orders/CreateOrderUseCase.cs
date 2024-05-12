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

    public async Task<Order> Execute(Cpf? customerCpf, List<(Guid productId, int quantity)> orderItems)
    {
        var order = new Order();
        if (customerCpf is not null)
        {
            var customer = await _customerRepository.FindByCpfAsync(customerCpf);
            if (customer is not null)
                order = new Order(Guid.NewGuid(), customer);
            else
                throw new EntityNotFoundException("Customer not found.");
        }

        foreach (var item in orderItems)
        {
            var product = await GetProduct(item.productId);
            order.AddOrderItem(product.Id, product.Name, product.Price, item.quantity);
        }

        order.Create();
        DomainEventTrigger.RaiseOrderCreated(order);
        return await _repository.CreateAsync(order);
    }

    private async Task<Product?> GetProduct(Guid productId)
    {
        var product = await _productRepository.FindByIdAsync(productId);
        if (product is null)
            throw new EntityNotFoundException($"Product '{productId}' not found.");

        return product;
    }
}
