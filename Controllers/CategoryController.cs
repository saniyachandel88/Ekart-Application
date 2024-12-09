using Ekart_Application.IServices;
using Ekart_web_Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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



        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategories();

                if (!categories.Any())
                {

                    return NotFound("No categories found.");
                }

                return Ok(categories);
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryReadDto>> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
        {
            try
            {
                // Ensure the input is valid
                if (categoryCreateDto == null)
                {
                    return BadRequest(new { error = "Category data cannot be null." });
                }

                // Call the service to create the category
                var message = await _categoryService.CreateCategory(categoryCreateDto);
                return Ok(new { message = "Category added successfully." });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = "Invalid data provided.", details = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Validation failed.", details = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Business logic error.", details = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Database error occurred.", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }


        //[Authorize(Roles = "admin")]
        //[HttpPost]
        //public async Task<ActionResult<CategoryReadDto>> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
        //{
        //    try
        //    {
        //        var message = await _categoryService.CreateCategory(categoryCreateDto);
        //        return Ok(new { message = "Category added successfully." });
        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        //    }
        //}

        [Authorize(Roles = "admin")]
        [HttpPut("{categoryId}")]
        public async Task<ActionResult> UpdateCategory(int categoryId, [FromBody] UpdateCategoryDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _categoryService.UpdateCategory(categoryId, updateDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(categoryId);

                if (!result)
                {

                    return NotFound($"Category with ID {categoryId} not found.");
                }


                return Ok($"Category with ID {categoryId} deleted successfully.");
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}