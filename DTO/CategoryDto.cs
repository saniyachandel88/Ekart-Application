using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category Name is Required")]
      
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[]? Picture { get; set; }
    }
}
