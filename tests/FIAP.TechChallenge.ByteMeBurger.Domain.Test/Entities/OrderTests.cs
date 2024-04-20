using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

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
        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(OrderStatus.None, order.Status);
        Assert.Equal(customerId, order.Customer.Id);
    }

    [Fact]
    public void Order_ConfirmEmptyOrder_ThrowsError()
    {
        // Arrange
        var order = new Order();
        
        // Act
        // Assert
        Assert.Throws<InvalidOperationException>(() => order.CreateOrder());
    }

    [Fact]
    public void Order_CreateOrder_UpdateStatus()
    {
        // Arrange
        var order = new Order(CustomerCpf);
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, new List<string>()));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, new List<string>()));
        
        // Act
        order.Validate();
        order.CreateOrder();

        // Assert
        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(OrderStatus.None, order.Status);
        Assert.True(order.CreationDate != default);
        Assert.Equal(CustomerCpf.Replace(".", "")
            .Replace("-", "")
            .Trim(), order.Customer.Id);
    }

    [Fact]
    public void Order_Initiate_NotConfirmed_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, new List<string>()));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, new List<string>()));
        
        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => order.InitiatePrepare());

        // Assert
        Assert.Equal("Cannot start preparing if order isn't confirmed.", exception.Message);
    }

    [Fact]
    public void Order_Finish_NotInitiated_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, new List<string>()));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, new List<string>()));
        order.CreateOrder();

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => order.FinishPreparing());

        // Assert
        Assert.Equal("Cannot Finish order if it's not Preparing yet.", exception.Message);
    }

    [Fact]
    public void Order_Deliver_NotFinished_ThrowsError()
    {
        // Arrange
        var order = new Order();
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, new List<string>()));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, new List<string>()));
        order.CreateOrder();
        order.ConfirmPayment();
        order.InitiatePrepare();
        
        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => order.DeliverOrder());

        // Assert
        Assert.Equal("Cannot Deliver order if it's not Finished yet.", exception.Message);
        Assert.NotNull(order.TrackingCode);
    }

    [Fact]
    public void Order_ValidOrder()
    {
        // Arrange
        var initDate = DateTime.UtcNow;
        DateTime preparingDate;
        DateTime doneDate;
        DateTime finishedDate;
        
        var order = new Order();
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, new List<string>()));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, new List<string>()));
        
        // Act
        order.CreateOrder();
        order.ConfirmPayment();
        order.InitiatePrepare();
        preparingDate = order.LastUpdate;
        order.FinishPreparing();
        doneDate = order.LastUpdate;
        order.DeliverOrder();
        finishedDate = order.LastUpdate;

        // Assert
        Assert.NotEqual(Guid.Empty.ToString(), order.Customer.Id);
        Assert.True(order.CreationDate > initDate, "Create date");
        Assert.True(preparingDate > order.CreationDate, "Preparing date");
        Assert.True(doneDate > preparingDate, "Done date");
        Assert.True(finishedDate > doneDate, "Finish date");
        Assert.Equal(OrderStatus.Finished, order.Status);
        Assert.Equal(22, order.Total);
        Assert.NotNull(order.TrackingCode);
    }

    [Fact]
    public void Order_FullOrderCode()
    {
        // Arrange
        var order = new Order();
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, new List<string>()));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, new List<string>()));
        order.AddProduct(new Product("soda", "the best", ProductCategory.Beverage, 12, new List<string>()));
        order.AddProduct(new Product("ice cream", "the best", ProductCategory.SweatsNTreats, 12, new List<string>()));

        // Act
        order.CreateOrder();
        order.ConfirmPayment();

        // Assert
        Assert.NotEqual(Guid.Empty.ToString(), order.Customer.Id);
        Assert.NotNull(order.TrackingCode);
        Assert.Contains("#", order.TrackingCode);
    }

    [Fact]
    public void Order_SimpleOrderCode()
    {
        // Arrange
        var order = new Order();
        order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, new List<string>()));
        order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, new List<string>()));
        order.AddProduct(new Product("soda", "the best", ProductCategory.Beverage, 12, new List<string>()));

        // Act
        order.CreateOrder();
        order.ConfirmPayment();

        // Assert
        Assert.NotEqual(Guid.Empty.ToString(), order.Customer.Id);
        Assert.NotNull(order.TrackingCode);
        Assert.DoesNotContain("#", order.TrackingCode);
    }

    [Fact]
    public void MultipleOrders()
    {
        // Arrange
        var codes = new List<string>();
        for (var i = 0; i < 100; i++)
        {
            var order = new Order();
            order.AddProduct(new Product("bread", "the best", ProductCategory.Meal, 10, new List<string>()));
            if (i % 2 == 0)
            {
                order.AddProduct(new Product("milk shake", "the best", ProductCategory.FriesAndSides, 12, default!));
                order.AddProduct(new Product("soda", "the best", ProductCategory.Beverage, 12, default!));
                order.AddProduct(new Product("ice cream", "the best", ProductCategory.SweatsNTreats, 12, default!));
            }
            order.CreateOrder();
            
            // Act
            order.ConfirmPayment();
            codes.Add(order.TrackingCode!);
            Thread.Sleep(20);
        }

        // Assert
        Assert.True(codes.Count == codes.Distinct().Count(), "Duplicated Order code.");
    }
}