using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;
    }
}
