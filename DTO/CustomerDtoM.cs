using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class CustomerDtoM
    {
        [Required(ErrorMessage = "Customer ID is required.")]
        [StringLength(50, ErrorMessage = "Customer ID cannot exceed 10 characters.")]
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
        public string CompanyName { get; set; }

        [StringLength(50, ErrorMessage = "Contact name cannot exceed 50 characters.")]
        public string ContactName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string Phone { get; set; }

        [StringLength(50, ErrorMessage = "City name cannot exceed 50 characters.")]
        public string City { get; set; }
    }
}
