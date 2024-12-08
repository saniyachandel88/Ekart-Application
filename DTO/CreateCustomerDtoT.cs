using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class CreateCustomerDtoT
    {

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(100, ErrorMessage = "Company name cannot be longer than 100 characters.")]
        public string CompanyName { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Contact name cannot be longer than 100 characters.")]
        public string? ContactName { get; set; }

        [StringLength(50, ErrorMessage = "Contact title cannot be longer than 50 characters.")]
        public string? ContactTitle { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string? Address { get; set; }

        [StringLength(100, ErrorMessage = "City cannot be longer than 100 characters.")]
        public string? City { get; set; }

        [StringLength(100, ErrorMessage = "Region cannot be longer than 100 characters.")]
        public string? Region { get; set; }

        [StringLength(20, ErrorMessage = "Postal code cannot be longer than 20 characters.")]
        public string? PostalCode { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters.")]
        public string? Country { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? Phone { get; set; }

        [Phone(ErrorMessage = "Invalid fax number.")]
        public string? Fax { get; set; }
    }
}
