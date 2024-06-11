namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

[Obsolete("This interface will be removed in the future.")]
public interface ICheckoutOrderUseCase
{
    Task Execute(Guid orderId);
}
