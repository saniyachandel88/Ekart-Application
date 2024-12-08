using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category Name is Required")]
        [RegularExpression(@"^[a-zA-z0-9\s]+$", ErrorMessage = "Category Name can only contain letters , numbers and spaces")]
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[]? Picture { get; set; }
    }
}
