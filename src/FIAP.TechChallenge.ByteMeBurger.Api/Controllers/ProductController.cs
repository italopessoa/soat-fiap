using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ReadOnlyCollection<ProductDto>>> Get(
            [FromQuery] ProductCategory? productCategory, CancellationToken cancellationToken)
        {
            var productsTask = productCategory.HasValue
                ? _productService.FindByCategory(productCategory!.Value)
                : _productService.GetAll();

            var products = await productsTask.WaitAsync(cancellationToken);
            return Ok(products.Select(p => new ProductDto(p))
                .ToList()
                .AsReadOnly());
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            if (Guid.Empty == id)
                return BadRequest();

            return await _productService.DeleteAsync(id) ? Ok() : NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(
            CreateProductCommandDto newProduct,
            CancellationToken cancellationToken)
        {
            if (newProduct.Price <= 0)
                return BadRequest("Price cannot be zero ou negative");

            try
            {
                var product = await _productService.CreateAsync(newProduct.Name, newProduct.Description,
                    newProduct.Category,
                    newProduct.Price, newProduct.Images);

                return Created($"/{product.Id}", product);
            }
            catch (Exception e)
            {
                return BadRequest("Unable to create the product");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductDto>> Update([FromRoute] Guid id,
            UpdateProductCommandDto updateProductCommandDto,
            CancellationToken cancellationToken)
        {
            if (Guid.Empty == id)
                return BadRequest("Invalid Id");

            var updated = await _productService.UpdateAsync(
                id,
                updateProductCommandDto.Name,
                updateProductCommandDto.Description,
                updateProductCommandDto.Category,
                updateProductCommandDto.Price,
                updateProductCommandDto.Images);

            return updated ? NoContent() : BadRequest("Unable to update the product");
        }
    }
}