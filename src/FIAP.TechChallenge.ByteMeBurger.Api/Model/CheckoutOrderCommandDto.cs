using System.ComponentModel.DataAnnotations;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;


/// <summary>
/// Command to checkout an order.
/// </summary>
/// <param name="Id">Order Id.</param>
public record CheckoutOrderCommandDto([Required] Guid Id);
