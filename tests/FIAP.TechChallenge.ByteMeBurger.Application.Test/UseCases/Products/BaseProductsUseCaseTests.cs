using Bmb.Domain.Core.Events;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;
using Bmb.Domain.Core.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.UseCases.Products;

public abstract class BaseProductsUseCaseTests
{
    protected readonly Mock<IProductRepository> _productRepository;
    protected readonly ICreateProductUseCase _createProductUseCase;
    protected readonly IUpdateProductUseCase _updateProductUseCase;
    protected readonly IDeleteProductUseCase _deleteProductUseCase;
    protected readonly IFindProductsByCategoryUseCase _findProductsByCategoryUseCase;
    protected readonly IGetAllProductsUseCase _getAllProductsUseCase;
    protected readonly Mock<IDispatcher> _dispatcher;

    public BaseProductsUseCaseTests()
    {
        _productRepository = new Mock<IProductRepository>();
        _dispatcher = new Mock<IDispatcher>();
        _createProductUseCase = new CreateProductUseCase(_productRepository.Object,  _dispatcher.Object);
        _updateProductUseCase = new UpdateProductUseCase(_productRepository.Object);
        _deleteProductUseCase = new DeleteProductUseCase(_productRepository.Object);
        _findProductsByCategoryUseCase = new FindProductsByCategoryUseCase(_productRepository.Object);
        _getAllProductsUseCase = new GetAllProductsUseCase(_productRepository.Object);
    }
}
