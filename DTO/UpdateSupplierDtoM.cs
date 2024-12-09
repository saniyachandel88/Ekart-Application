using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class UpdateSupplierDtoM
    {
        [Required(ErrorMessage = "Supplier ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Supplier ID must be a positive number.")]
        public int SupplierId { get; set; }
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string? City { get; set; }

        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid postal code format. It should be '12345' or '12345-6789'.")]
        public string? PostalCode { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        public string? Country { get; set; }
    }
}
