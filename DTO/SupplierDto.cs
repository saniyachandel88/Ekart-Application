using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class SupplierDto
    {
        [Required(ErrorMessage = "Supplier ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Supplier ID must be a positive number.")]
        public int SupplierId { get; set; }

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

        [StringLength(20, ErrorMessage = "Fax cannot exceed 20 characters.")]
        public string? Fax { get; set; }

        [Url(ErrorMessage = "Invalid URL format for HomePage.")]
        [StringLength(200, ErrorMessage = "HomePage URL cannot exceed 200 characters.")]
        public string? HomePage { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Product count must be a non-negative number.")]
        public int ProductCount { get; set; }
    }
}
