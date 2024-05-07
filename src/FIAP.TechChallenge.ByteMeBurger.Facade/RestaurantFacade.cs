using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

namespace FIAP.TechChallenge.ByteMeBurger.Facade;

[Obsolete(
    "This class can be used to aggregate services and expose them to the application layer. Working like an adapter for services.")]
public class RestaurantFacade
{
    public RestaurantFacade(ICustomerService customerService, IProductService productService,
        IOrderService orderService)
    {
    }
}