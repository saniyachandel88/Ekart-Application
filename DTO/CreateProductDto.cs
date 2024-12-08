using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters.")]
        public string ProductName { get; set; }

        public int? SupplierId { get; set; }

        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Quantity per unit is required.")]
        [StringLength(50, ErrorMessage = "Quantity per unit cannot be longer than 50 characters.")]
        public string QuantityPerUnit { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
        public decimal? UnitPrice { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "Units in stock cannot be negative.")]
        public short? UnitsInStock { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "Units on order cannot be negative.")]
        public short? UnitsOnOrder { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "Reorder level cannot be negative.")]
        public short? ReorderLevel { get; set; }

        public bool? Discontinued { get; set; }

        // Custom validation for SupplierId and CategoryId
        public bool IsValidSupplierAndCategory()
        {
            if (SupplierId.HasValue && !CategoryId.HasValue)
            {
                // If SupplierId is provided, CategoryId should also be provided
                return false;
            }
            return true;
        }
    }
}

