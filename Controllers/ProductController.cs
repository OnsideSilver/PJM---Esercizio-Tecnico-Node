using Node_ApiService_Test.Controllers.ControllerExtensions;
using Node_ApiService_Test.DTOs;
using Node_ApiService_Test.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Node_ApiService_Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        // Constructor injection
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET all products
        [HttpGet ("all")]
        public ActionResult<IEnumerable<ProductDto>> GetProducts()
        {
            var products = _productService.GetAllProducts(); // Get all products from the service
            return Ok(products);
        }

        // GET a product
        [HttpGet("id or name")]
        public ActionResult<ProductDto> GetProduct([FromQuery] Guid? id, [FromQuery] string? name)
        {
            ProductDto product = null;

            // If both ID and Name are provided, prioritize ID
            if (id.HasValue)
            {
                product = _productService.ReadId(id.Value);
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                product = _productService.ReadName(name);
            }

            if (product == null)
            {
                return this.NotFoundWithProducts(_productService.GetAllProducts());
            }

            return Ok(product);
        }

        // POST a new product
        [HttpPost("{name}/{price}")]
        public ActionResult<ProductDto> CreateProduct(string name, decimal price)
        {
            var productDto = new ProductDto
            {
                Name = name,
                Price = price
            };

            var createdProduct = _productService.Create(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }

        // PUT an existing product
        [HttpPut("{id}")]
        public ActionResult UpdateProduct(Guid id, ProductDto productDto)
        {
            var product = _productService.Update(id, productDto);
            if (product == null)
            {
                return this.NotFoundWithProducts(_productService.GetAllProducts());
            }
            return Ok(product);
        }

        // DELETE a product       
        [HttpDelete("id or name")]
        [Authorize] //Requires Authentication (check the README for more info)
        public ActionResult DeleteProduct([FromQuery] Guid? id, [FromQuery] string? name)
        {
            // If both ID and Name are provided, prioritize ID
            if (!id.HasValue && string.IsNullOrEmpty(name))
            {
                return this.NotFoundWithProducts(_productService.GetAllProducts());
            }

           else if (id.HasValue && !string.IsNullOrEmpty(name) || id.HasValue && string.IsNullOrEmpty(name))
            {
                bool deleted = _productService.DeleteId(id.Value);
                if (!deleted)
                {
                    return this.NotFoundWithProducts(_productService.GetAllProducts());
                }
                return Ok(new { message = "Product successfully deleted. N.B: In the case you have filled both Id and Name of 2 different products, only the product associated with the Id has been deleted." });
            }

            else if (!string.IsNullOrEmpty(name))
            {
                bool deleted = _productService.DeleteName(name);
                if (!deleted)
                {
                    return this.NotFoundWithProducts(_productService.GetAllProducts());
                }
                return Ok(new { message = "Product successfully deleted."});
            }

            return BadRequest("Invalid request.");
        }
    }
}
