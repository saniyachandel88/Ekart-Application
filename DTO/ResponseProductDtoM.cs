using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class ResponseProductDtoM
    {
        [Required(ErrorMessage = "Product ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be a positive number.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string ProductName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Supplier ID must be a positive number.")]
        public int? SupplierId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive number.")]
        public int? CategoryId { get; set; }

        [StringLength(50, ErrorMessage = "Quantity per unit cannot exceed 50 characters.")]
        public string QuantityPerUnit { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be a non-negative number.")]
        public decimal? UnitPrice { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "Units in stock must be a non-negative number.")]
        public short UnitsInStock { get; set; }

        [Required(ErrorMessage = "Discontinued status is required.")]
        public bool? Discontinued { get; set; }
    }
}
