using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ReadOnlyCollection<ProductDto>>> Get(
            [FromQuery] ProductCategory? productCategory, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting products by category: {ProductCategory}", productCategory);
            var productsTask = productCategory.HasValue
                ? _productService.FindByCategory(productCategory!.Value)
                : _productService.GetAll();

            var products = await productsTask.WaitAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} products", products.Count);
            return Ok(products.Select(p => new ProductDto(p))
                .ToList()
                .AsReadOnly());
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting product with ID: {ProductId}", id);
            if (Guid.Empty == id)
                return BadRequest();

            var result = await _productService.DeleteAsync(id);
            if (result)
            {
                _logger.LogInformation("Product with ID: {ProductId} deleted", id);
                return Ok();
            }
            else
            {
                _logger.LogWarning("Product with ID: {ProductId} not found", id);
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(
            CreateProductCommandDto newProduct,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating product with name: {ProductName}", newProduct.Name);
            if (newProduct.Price <= 0)
                return BadRequest("Price cannot be zero ou negative.");

            try
            {
                var product = await _productService.CreateAsync(newProduct.Name, newProduct.Description,
                    newProduct.Category,
                    newProduct.Price, newProduct.Images);

                _logger.LogInformation("Product with ID: {ProductId} created", product.Id);
                return Created($"/{product.Id}", product);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating product with name: {ProductName}", newProduct.Name);
                return BadRequest("Unable to create the product.");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductDto>> Update([FromRoute] Guid id,
            UpdateProductCommandDto updateProductCommandDto,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating product with ID: {ProductId}", id);
            if (Guid.Empty == id)
                return BadRequest("Invalid Id.");

            var updated = await _productService.UpdateAsync(
                id,
                updateProductCommandDto.Name,
                updateProductCommandDto.Description,
                updateProductCommandDto.Category,
                updateProductCommandDto.Price,
                updateProductCommandDto.Images);

            if (updated)
            {
                _logger.LogInformation("Product with ID: {ProductId} updated", id);
                return NoContent();
            }

            _logger.LogWarning("Unable to update product with ID: {ProductId}", id);
            return BadRequest("Unable to update the product.");
        }
    }
}