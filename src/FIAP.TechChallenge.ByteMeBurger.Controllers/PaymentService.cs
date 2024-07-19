using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers;

public class PaymentService : IPaymentService
{
    private readonly ICreatePaymentUseCase _createOrderPaymentUseCase;
    private readonly IUpdatePaymentStatusUseCase _updatePaymentStatusUseCase;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGatewayFactoryMethod _paymentGatewayFactory;
    private readonly IUpdateOrderPaymentUseCase _updateOrderPaymentUseCase;

    public PaymentService(ICreatePaymentUseCase createOrderPaymentUseCase,
        IUpdatePaymentStatusUseCase updatePaymentStatusUseCase,
        IPaymentRepository paymentRepository,
        IPaymentGatewayFactoryMethod paymentGatewayFactory,
        IUpdateOrderPaymentUseCase updateOrderPaymentUseCase)
    {
        _createOrderPaymentUseCase = createOrderPaymentUseCase;
        _updatePaymentStatusUseCase = updatePaymentStatusUseCase;
        _paymentRepository = paymentRepository;
        _paymentGatewayFactory = paymentGatewayFactory;
        _updateOrderPaymentUseCase = updateOrderPaymentUseCase;
    }

    public async Task<PaymentDto> CreateOrderPaymentAsync(CreateOrderPaymentRequestDto command)
    {
        var payment = await _createOrderPaymentUseCase.Execute(command.OrderId, command.PaymentType);
        if (payment is null)
            return null;

        await _paymentRepository.SaveAsync(payment);
        await _updateOrderPaymentUseCase.Execute(payment.OrderId, payment.Id);
        return payment.FromEntityToDto();
    }

    public async Task<PaymentDto?> GetPaymentAsync(Guid id)
    {
        var payment = await _paymentRepository.GetPaymentAsync(new PaymentId(id));
        return payment?.FromEntityToDto();
    }

    public async Task<PaymentDto?> GetPaymentAsync(string externalReference, PaymentType paymentType)
    {
        var payment = await _paymentRepository.GetPaymentAsync(externalReference, paymentType);
        return payment?.FromEntityToDto();
    }

    public async Task<bool> SyncPaymentStatusWithGatewayAsync(string externalReference, PaymentType paymentType)
    {
        var payment = await _paymentRepository.GetPaymentAsync(externalReference, paymentType);
        if (payment is null)
            return false;

        var paymentGateway = _paymentGatewayFactory.Create(paymentType);
        var paymentStatus = await paymentGateway.GetPaymentStatusAsync(externalReference);
        if (paymentStatus is null)
            return false;

        await _updatePaymentStatusUseCase.Execute(payment, paymentStatus.Value);
        return true;
    }
}
