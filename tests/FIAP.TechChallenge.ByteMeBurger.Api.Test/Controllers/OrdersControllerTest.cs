// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using AutoFixture;
using FIAP.TechChallenge.ByteMeBurger.Api.Controllers;
using FIAP.TechChallenge.ByteMeBurger.Api.Model.Orders;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Test.Controllers;

[TestSubject(typeof(OrdersController))]
public class OrdersControllerTest
{
    private readonly Mock<IOrderService> _serviceMock;
    private readonly OrdersController _target;
    private const string FakeCpf = "863.917.790-23";

    public OrdersControllerTest()
    {
        _serviceMock = new Mock<IOrderService>();
        _target = new OrdersController(_serviceMock.Object, Mock.Of<ILogger<OrdersController>>(), new MockHybridCache());
    }

    [Fact]
    public async void GetAll_Success()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "productA", "product description", ProductCategory.Drink, 10, []);
        var orderId = Guid.NewGuid();
        var customer = new Customer(Guid.NewGuid(), FakeCpf);
        var expectedOrder = new Order(orderId, customer);
        expectedOrder.SetTrackingCode(new OrderTrackingCode("code"));
        expectedOrder.AddOrderItem(product.Id, product.Name, product.Price, 10);

        var orders = new[]
        {
            expectedOrder
        };
        var expectedOrdersDto = new OrderListViewModel[]
        {
            expectedOrder.ToOrderListViewModel()
        };

        _serviceMock.Setup(s => s.GetAllAsync(false))
            .ReturnsAsync(orders.ToList().AsReadOnly);

        // Act
        var response = await _target.Get(false, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedOrdersDto);

            _serviceMock.Verify(o => o.GetAllAsync(false), Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async void Create_WithCustomer_Success()
    {
        // Arrange
        var customer = new Customer(FakeCpf, "name", "email@emil.com");
        var fixture = new Fixture();
        var orderId = Guid.NewGuid();
        var products = new Product[]
        {
            new(Guid.NewGuid(), "productA", "product description", ProductCategory.Drink, 10, [])
        };
        var chosenProduct = products.First();

        var createOrderCommand = fixture.Build<CreateOrderRequest>()
            .With(c => c.Cpf, FakeCpf)
            .Create();

        var expectedOrder = new Order(orderId, customer);
        expectedOrder.AddOrderItem(chosenProduct.Id, chosenProduct.Name, chosenProduct.Price, 10);
        expectedOrder.SetTrackingCode(new OrderTrackingCode("code"));

        _serviceMock.Setup(s => s.CreateAsync(It.IsAny<string?>(),
                It.IsAny<List<SelectedProduct>>()))
            .ReturnsAsync(expectedOrder);

        // Act
        var response = await _target.Post(createOrderCommand, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdOrder = response.Result.As<CreatedAtActionResult>().Value.As<NewOrderDto>();

            createdOrder.Should()
                .BeEquivalentTo(new { expectedOrder.Id, TrackingCode = "code" },
                    options => options.ExcludingMissingMembers());

            _serviceMock.Verify(
                s => s.CreateAsync(It.IsAny<string?>(),
                    It.IsAny<List<SelectedProduct>>()),
                Times.Once);

            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async void Create_NoCustomer_Success()
    {
        // Arrange
        var fixture = new Fixture();
        var orderId = Guid.NewGuid();
        var products = new Product[]
        {
            new(Guid.NewGuid(), "productA", "product description", ProductCategory.Drink, 10, [])
        };
        var chosenProduct = products.First();

        var createOrderCommand = fixture.Build<CreateOrderRequest>()
            .With(c => c.Cpf, FakeCpf)
            .Create();

        var expectedOrder = new Order(orderId, null);
        expectedOrder.AddOrderItem(chosenProduct.Id, chosenProduct.Name, chosenProduct.Price, 10);
        expectedOrder.SetTrackingCode(new OrderTrackingCode("code"));

        _serviceMock.Setup(s => s.CreateAsync(It.IsAny<string?>(),
                It.IsAny<List<SelectedProduct>>()))
            .ReturnsAsync(expectedOrder);

        // Act
        var response = await _target.Post(createOrderCommand, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdOrder = response.Result.As<CreatedAtActionResult>().Value.As<NewOrderDto>();

            createdOrder.Should()
                .BeEquivalentTo(new { expectedOrder.Id, TrackingCode = "code" },
                    options => options.ExcludingMissingMembers());

            _serviceMock.Verify(
                s => s.CreateAsync(It.IsAny<string?>(),
                    It.IsAny<List<SelectedProduct>>()),
                Times.Once);

            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async void Get_Detail_Success()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "productA", "product description", ProductCategory.Drink, 10, []);
        var customerId = Guid.NewGuid();
        var expectedOrder = new Order(customerId, null);
        expectedOrder.SetTrackingCode(new OrderTrackingCode("code"));
        expectedOrder.AddOrderItem(product.Id, product.Name, product.Price, 10);

        var expectedOrderDto = expectedOrder.ToOrderViewModel();

        _serviceMock.Setup(s => s.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedOrder);

        // Act
        var response = await _target.Get(expectedOrder.Id, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedOrderDto);

            _serviceMock.Verify(o => o.GetAsync(It.IsAny<Guid>()), Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async void Get_Detail_NotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(default(Order));

        // Act
        var response = await _target.Get(Guid.NewGuid(), CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<NotFoundResult>();

            _serviceMock.Verify(o => o.GetAsync(It.IsAny<Guid>()), Times.Once);
            _serviceMock.VerifyAll();
        }
    }

    [Fact]
    public async void Get_Detail_InvalidId_BadRequest()
    {
        // Arrange
        // Act
        var response = await _target.Get(Guid.Empty, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<BadRequestObjectResult>();

            _serviceMock.Verify(o => o.GetAsync(It.IsAny<Guid>()), Times.Never);
            _serviceMock.VerifyAll();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void UpdateOrderStatusAsync(bool success)
    {
        // Arrange
        _serviceMock.Setup(s => s.UpdateStatusAsync(It.IsAny<Guid>(), It.IsAny<OrderStatus>()))
            .ReturnsAsync(success)
            .Verifiable();

        // Act
        var response = await _target.Patch(Guid.NewGuid(),
            new UpdateOrderStatusRequest() { Status = OrderStatusViewModel.Ready }, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            if (success)
                response.Result.Should().BeOfType<NoContentResult>();
            else
                response.Result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
