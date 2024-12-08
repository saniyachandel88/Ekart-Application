using AutoMapper;
using Ekart_Web_App.Data;
using Ekart_Web_App.Models;
using Ekart_web_Application.DTO;
using Microsoft.EntityFrameworkCore;

namespace Ekart_Application.IServices.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly EkartProjectContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(EkartProjectContext context, IMapper mapper, ILogger<CategoryService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;

        }
        public async Task<CategoryCreateDto> CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            try
            {
                if (categoryCreateDto == null)
                    throw new ArgumentNullException(nameof(categoryCreateDto), "Category cannot be null");

                // Convert picture to byte array
                byte[]? pictureBytes = categoryCreateDto.GetPictureBytes();

                if (pictureBytes == null || pictureBytes.Length == 0)
                    throw new ArgumentException("Picture is required and must be valid Base64 data");

                // Check for duplicate category name
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == categoryCreateDto.CategoryName.ToLower());

                if (existingCategory != null)
                    throw new InvalidOperationException("A category with this name already exists");

                // Map DTO to domain model
                var category = _mapper.Map<Category>(categoryCreateDto);
                category.Picture = pictureBytes;

                // Add to context and save changes
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return _mapper.Map<CategoryCreateDto>(category);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while creating category");
                throw new Exception("Database error occurred.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category");
                throw;
            }
        }



        public async Task<IEnumerable<CategoryReadDto>> GetCategories()
        {
            try
            {
                var categories = await _context.Categories
                    .Select(c => new CategoryReadDto
                    {
                        CategoryId = c.CategoryId,
                        CategoryName = c.CategoryName,
                        Description = c.Description,
                        ProductCount = _context.Products.Count(p => p.CategoryId == c.CategoryId) // More efficient counting
                    }).ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching categories");
                throw new Exception("An error occurred while fetching categories. Please try again later.");
            }
        }


        public async Task UpdateCategory(int categoryId, UpdateCategoryDto updateDto)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
            }

            // Update only the Description
            category.Description = updateDto.Description;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            // Check if category exists
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (category == null)
            {
                return false;
            }

            // Check if category has associated products
            bool hasProducts = await _context.Products
                .AnyAsync(p => p.CategoryId == categoryId);

            if (hasProducts)
            {
                throw new InvalidOperationException(
                    "Cannot delete category with existing products.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
