using Ekart_Application.IServices;
using Ekart_web_Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ekart_Application.Controllers
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
        [Authorize(Roles = "admin")]
        [HttpPatch("edit/{productId}")]
        public async Task<IActionResult> PatchProductAsync(int productId, [FromBody] PatchProductDto patchProductDto)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors to the client
                return BadRequest(ModelState);
            }
            await _productService.PatchProductAsync(productId, patchProductDto);
            return NoContent();
        }
        [Authorize(Roles = "admin")]
        [HttpGet("OutOfStock")]
        public async Task<IActionResult> GetOutOfStockProductsAsync()
        {
            var products = await _productService.GetOutOfStockProductsAsync();
            if (products == null || !products.Any())
            {
                return NotFound("No Products  are out of stocks");
            }
            return Ok(products);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("edit/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] PatchProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _productService.UpdateProductAsync(productId, updateProductDto);
                return NoContent(); // Success response (204 No Content)
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "admin")]
        // GET: api/products/unitbystock/{unitsInStock}
        [HttpGet("UnitInStock/{unitInStock}")]
        public async Task<IActionResult> GetProductsByUnitInStock(int unitInStock)
        {
            var products = await _productService.GetProductsByUnitInStockAsync(unitInStock);

            if (products == null || !products.Any())
            {
                return NotFound("No products found for the given Units in Stock.");
            }

            return Ok(products);
        }

        [Authorize(Roles = "admin")]
        // GET: api/products/bysupplier/{supplierId}
        [HttpGet("BySupplier/{supplierId}")]
        public async Task<IActionResult> GetProductsBySupplierId(int supplierId)
        {
            var products = await _productService.GetProductsBySupplierId(supplierId);

            if (products == null || !products.Any())
            {
                return NotFound("No products found for the given Supplier ID.");
            }

            return Ok(products);
        }
        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productId = await _productService.AddProductAsync(productDto);
            return Ok(new { Message = "Product added successfully", ProductId = productId });
        }
        [Authorize(Roles = "admin,customer")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        [Authorize(Roles = "admin")]
        [HttpGet("DiscontinuedProduct")]
        public async Task<IActionResult> GetDiscontinuedProducts()
        {
            var discontinuedProducts = await _productService.GetDiscontinuedProductsAsync();

            if (discontinuedProducts != null)
            {
                return Ok(discontinuedProducts);
            }
            else
            {
                return StatusCode(500, "Unable to retrieve discontinued products.");
            }
        }
        [Authorize(Roles = "admin")]
        [HttpGet("UnitsOnOrder")]
        public async Task<IActionResult> GetProductsByUnitsOnOrder()
        {
            var products = await _productService.GetProductsByUnitsOnOrderAsync();

            if (products == null || !products.Any())
            {
                return NotFound("No products found with UnitsOnOrder greater than 0.");
            }

            return Ok(products);
        }
        [Authorize(Roles = "admin")]
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetAllProductsByCategoryIdAsync(int categoryId)
        {
            var products = await _productService.GetAllProductsByCategoryIdAsync(categoryId);

            if (!products.Any())
            {
                return NotFound($"No products found for CategoryId: {categoryId}");
            }

            return Ok(products);
        }
        [Authorize(Roles = "admin")]
        [HttpGet("ByCategoryName/{categoryName}")]
        public async Task<ActionResult<IEnumerable<ProductDtoM>>> GetProductsByCategoryName(string categoryName)
        {
            var products = await _productService.GetProductsByCategoryNameAsync(categoryName);

            if (!products.Any())
            {
                return NotFound(new { message = $"No products found for category '{categoryName}'" });
            }

            return Ok(products);
        }
        [Authorize(Roles = "admin")]
        [HttpGet("ProductBySupplier/{companyName}")]
        public async Task<ActionResult<IEnumerable<ProductDtoM>>> GetProductsBySupplierName(string companyName)
        {
            var products = await _productService.GetProductsBySupplierNameAsync(companyName);

            if (!products.Any())
            {
                return NotFound(new { message = $"No products found for supplier '{companyName}'" });
            }

            return Ok(products);
        }

        [HttpGet("AverageUnitPrice")]
        public async Task<ActionResult<decimal>> GetProductsAverageAsync()
        {
            var averagePrice = await _productService.GetProductsAverageAsync();
            return Ok(averagePrice);
        
    }
}
}
