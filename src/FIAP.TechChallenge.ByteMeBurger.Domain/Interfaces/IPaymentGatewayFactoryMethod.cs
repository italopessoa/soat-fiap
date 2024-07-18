using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface IPaymentGatewayFactoryMethod
{
    IPaymentGateway Create(PaymentType paymentType);
}
