using FIAP.TechChallenge.ByteMeBurger.Api.Model.Products;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers;

/// <summary>
/// Products controller
/// </summary>
/// <param name="productService">Products service (port implementation). </param>
/// <param name="logger">Logger</param>
[Route("api/[controller]")]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Produces("application/json")]
[Consumes("application/json")]
public class ProductsController(IProductService productService, ILogger<ProductsController> logger)
    : ControllerBase
{
    /// <summary>
    /// Get all products or filter by category
    /// </summary>
    /// <param name="productCategory">Product category.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Product list.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ProductDto>>> Get(
        [FromQuery] ProductCategoryDto? productCategory, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting products by category: {ProductCategory}", productCategory);
        var productsTask = productCategory.HasValue
            ? productService.FindByCategory((ProductCategory)productCategory.Value)
            : productService.GetAll();

        var products = await productsTask.WaitAsync(cancellationToken);
        logger.LogInformation("Retrieved {Count} products", products.Count);
        return Ok(products);
    }

    /// <summary>
    /// Delete existing product
    /// </summary>
    /// <param name="id">Product Id.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result</returns>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting product with ID: {ProductId}", id);
        if (Guid.Empty == id)
            return BadRequest();

        var result = await productService.DeleteAsync(id);
        if (result)
        {
            logger.LogInformation("Product with ID: {ProductId} deleted", id);
            return Ok();
        }
        else
        {
            logger.LogWarning("Product with ID: {ProductId} not found", id);
            return NoContent();
        }
    }

    /// <summary>
    /// Create new product
    /// </summary>
    /// <param name="newProduct">Create product params.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Product</returns>
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(
        CreateProductRequest newProduct,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating product with name: {ProductName}", newProduct.Name);
        if (newProduct.Price <= 0)
            return BadRequest("Price cannot be zero ou negative.");

        try
        {
            var product = await productService.CreateAsync(newProduct.Name, newProduct.Description,
                (ProductCategory)newProduct.Category,
                newProduct.Price, newProduct.Images);

            logger.LogInformation("Product with ID: {ProductId} created", product.Id);
            return Created($"/{product.Id}", product);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating product with name: {ProductName}", newProduct.Name);
            return BadRequest("Unable to create the product.");
        }
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="id">Product Id.</param>
    /// <param name="updateProductRequest">Update product params.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated product</returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductDto>> Update([FromRoute] Guid id,
        UpdateProductRequest updateProductRequest,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating product with ID: {ProductId}", id);
        if (Guid.Empty == id)
            return BadRequest("Invalid Id.");

        var updated = await productService.UpdateAsync(
            id,
            updateProductRequest.Name,
            updateProductRequest.Description,
            (ProductCategory)updateProductRequest.Category,
            updateProductRequest.Price,
            updateProductRequest.Images);

        if (updated)
        {
            logger.LogInformation("Product with ID: {ProductId} updated", id);
            return NoContent();
        }

        logger.LogWarning("Unable to update product with ID: {ProductId}", id);
        return BadRequest("Unable to update the product.");
    }
}
