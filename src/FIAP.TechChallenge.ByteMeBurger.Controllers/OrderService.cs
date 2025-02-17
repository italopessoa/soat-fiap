using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;
using Bmb.Orders.Domain.Contracts;
using Bmb.Orders.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers;

public class OrderService(
    ICreateOrderUseCase createOrderUseCase,
    IGetOrderDetailsUseCase getOrderDetailsUseCase,
    IOrderGetAllUseCase orderGetAllUseCase,
    IOrderRepository orderRepository,
    IUseCase<UpdateOrderStatusRequest, bool> updateOrderStatusUseCase,
    IUpdateOrderPaymentUseCase updateOrderPaymentUseCase)
    : IOrderService
{
    public async Task<NewOrderDto> CreateAsync(CustomerDto? customer, List<SelectedProduct> selectedProducts)
    {
        var order = await createOrderUseCase.Execute(customer.ToDomain(), selectedProducts);

        await orderRepository.CreateAsync(order);
        return order.FromEntityToCreatedDto();
    }

    public async Task<IReadOnlyCollection<OrderListItemDto>> GetAllAsync(bool listAll)
    {
        var orders = await orderGetAllUseCase.Execute(listAll);
        return orders.FromDomainToDto();
    }

    public async Task<OrderDetailDto?> GetAsync(Guid id)
    {
        var order = await getOrderDetailsUseCase.Execute(id);
        return order.FromEntityToDto();
    }

    public async Task<bool> UpdateStatusAsync(Guid orderId, OrderStatus newStatus)
    {
        return await updateOrderStatusUseCase.ExecuteAsync(new UpdateOrderStatusRequest(orderId, newStatus));
    }

    public async Task<bool> UpdateOrderPaymentAsync(Guid orderId, PaymentId paymentId)
    {
        return await updateOrderPaymentUseCase.Execute(orderId, paymentId);
    }
}
