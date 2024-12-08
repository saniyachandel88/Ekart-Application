using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class CreateSupplierDtoS
    {
        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
        public string CompanyName { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Contact name cannot exceed 50 characters.")]
        public string? ContactName { get; set; }

        [StringLength(50, ErrorMessage = "Contact title cannot exceed 50 characters.")]
        public string? ContactTitle { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string? City { get; set; }

        [StringLength(50, ErrorMessage = "Region cannot exceed 50 characters.")]
        public string? Region { get; set; }

        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters.")]
        public string? PostalCode { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        public string? Country { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        public string? Phone { get; set; }

        [StringLength(20, ErrorMessage = "Fax number cannot exceed 20 characters.")]
        public string? Fax { get; set; }

        [Url(ErrorMessage = "Invalid URL format for homepage.")]
        public string? HomePage { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; } = null!;
    }
}
