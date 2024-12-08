using Ekart_web_Application.DTO;

namespace Ekart_Application.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryReadDto>> GetCategories();
        Task<CategoryCreateDto> CreateCategory(CategoryCreateDto categoryCreateDto);
        Task UpdateCategory(int categoryId, UpdateCategoryDto updateDto);
        Task<bool> DeleteCategoryAsync(int categoryId);

    }
}
