using Ekart_Application.IServices;
using Ekart_web_Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ekart_Application.Controllers
{
   
        [Authorize(Roles = "admin")]
        [ApiController]
        [Route("api/[controller]")]
        public class CategoryController : ControllerBase
        {
            private readonly ICategoryService _categoryService;

            public CategoryController(ICategoryService categoryService)
            {
                _categoryService = categoryService;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetCategories()
            {
                var categories = await _categoryService.GetCategories();
                if (!categories.Any())
                    return NotFound("No categories found.");

                return Ok(categories);
            }

            [HttpPost]
            public async Task<ActionResult<CategoryReadDto>> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
            {
                await _categoryService.CreateCategory(categoryCreateDto);
                return Ok(new { message = "Category added successfully." });
            }

            [HttpPut("{categoryId}")]
            public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] UpdateCategoryDto updateDto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _categoryService.UpdateCategory(categoryId, updateDto);
                return NoContent();
            }

            [HttpDelete("{categoryId}")]
            public async Task<IActionResult> DeleteCategory(int categoryId)
            {
                var result = await _categoryService.DeleteCategoryAsync(categoryId);
                if (!result)
                    return NotFound($"Category with ID {categoryId} not found.");

                return Ok($"Category with ID {categoryId} deleted successfully.");
            }
        }

    
}
