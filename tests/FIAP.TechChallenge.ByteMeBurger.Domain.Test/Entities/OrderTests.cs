using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Test.Entities;

public class OrderTests
{
    private const string CustomerCpf = "946.571.740-10";

    [Fact]
    public void Order_NewOrder_HasId()
    {
        // Arrange
        var customerId = Guid.NewGuid().ToString();

        // Act
        var order = new Order(customerId);

        // Assert
        using (new AssertionScope())
        {
            order.Id.Should().NotBe(Guid.Empty);
            order.Status.Should().Be(OrderStatus.None);
            order.Customer.Id.Should().Be(customerId);
        }
    }

    [Fact]
    public void Order_CheckoutEmptyOrder_ThrowsError()
    {
        // Arrange
        var order = new Order();

        // Act
        var func = () => order.Checkout();

        // Assert
        func.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Order_CheckoutOrder_UpdateStatus()
    {
        // Arrange
        var order = new Order(CustomerCpf);
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, []));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, []));

        // Act
        order.Checkout();

        // Assert
        using (new AssertionScope())
        {
            order.Id.Should().NotBe(Guid.Empty);
            order.Status.Should().Be(OrderStatus.None);
            order.CreationDate.Should().NotBe(default);
            order.Customer.Id.Should().Be(CustomerCpf.Replace(".", "")
                .Replace("-", "")
                .Trim(), order.Customer.Id);
        }
    }

    [Fact]
    public void Order_Initiate_NotConfirmed_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, []));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, []));

        // Act
        var func = () => order.InitiatePrepare();

        // Assert
        func.Should().Throw<InvalidOperationException>().And.Message.Should()
            .Be("Cannot start preparing if order isn't confirmed.");
    }

    [Fact]
    public void Order_Finish_NotInitiated_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, []));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, []));
        order.Checkout();

        // Act
        var func = () => order.FinishPreparing();

        // Assert
        func.Should().Throw<InvalidOperationException>().And.Message.Should()
            .Be("Cannot Finish order if it's not Preparing yet.");
    }

    [Fact]
    public void Order_Deliver_NotFinished_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, []));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, []));
        order.Checkout();
        order.ConfirmPayment();
        order.InitiatePrepare();

        // Act
        var func = () => order.DeliverOrder();

        // Assert
        func.Should().Throw<InvalidOperationException>().And.Message.Should()
            .Be("Cannot Deliver order if it's not Finished yet.");
        order.TrackingCode.Should().NotBeEmpty();
    }

    [Fact]
    public void Order_ValidOrder()
    {
        // Arrange
        var initDate = DateTime.UtcNow;

        var order = new Order();
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, []));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, []));

        // Act
        order.Checkout();
        order.ConfirmPayment();
        order.InitiatePrepare();
        var preparingDate = order.LastUpdate;
        order.FinishPreparing();
        var doneDate = order.LastUpdate;
        order.DeliverOrder();
        var finishedDate = order.LastUpdate;

        // Assert
        using (new AssertionScope())
        {
            order.Customer.Id.Should().NotBe(Guid.Empty.ToString());
            order.CreationDate.Should().BeAfter(initDate);
            order.CreationDate.Should().BeBefore(preparingDate);
            doneDate.Should().BeAfter(preparingDate);
            finishedDate.Should().BeAfter(doneDate);
            order.Status.Should().Be(OrderStatus.Finished);
            order.Total.Should().Be(22);
            order.TrackingCode.Should().NotBeEmpty();
        }
    }

    [Fact]
    public void Order_FullOrderCode()
    {
        // Arrange
        var order = new Order();
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, []));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, []));
        order.AddProduct(new Product("soda", "the best", ProductCategory.Beverage, 12, []));
        order.AddProduct(new Product("ice cream", "the best", ProductCategory.SweatsNTreats, 12, []));

        // Act
        order.Checkout();
        order.ConfirmPayment();

        // Assert
        using (new AssertionScope())
        {
            order.Customer.Id.Should().NotBe(Guid.Empty.ToString());
            order.TrackingCode.Should().NotBeNull().And.Contain("#");
        }
    }

    [Fact]
    public void Order_SimpleOrderCode()
    {
        // Arrange
        var order = new Order();
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, []));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, []));
        order.AddProduct(new Product("soda", "the best", ProductCategory.Beverage, 12, []));

        // Act
        order.Checkout();
        order.ConfirmPayment();

        // Assert
        using (new AssertionScope())
        {
            order.Customer.Id.Should().NotBe(Guid.Empty.ToString());
            order.TrackingCode.Should().NotBeNull().And.NotContain("#");
        }
    }

    [Fact]
    public void MultipleOrders()
    {
        // Arrange
        var codes = new List<string>();
        for (var i = 0; i < 50; i++)
        {
            var order = new Order();
            order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, []));
            if (i % 2 == 0)
            {
                order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, default!));
                order.AddProduct(new Product("soda", "the best", ProductCategory.Beverage, 12, default!));
                order.AddProduct(new Product("ice cream", "the best", ProductCategory.SweatsNTreats, 12, default!));
            }

            order.Checkout();

            // Act
            order.ConfirmPayment();
            codes.Add(order.TrackingCode!);
            Thread.Sleep(20);
        }

        // Assert
        codes.Count.Should().Be(codes.Distinct().Count());
    }
}