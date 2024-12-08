using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class PatchProductDto
    {
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string? ProductName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid Supplier ID.")]
        public int? SupplierId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid Category ID.")]
        public int? CategoryId { get; set; }

        [StringLength(100, ErrorMessage = "Quantity per unit cannot exceed 100 characters.")]
        public string? QuantityPerUnit { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
        public decimal? UnitPrice { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "Units in stock cannot be negative.")]
        public short? UnitsInStock { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "Units on order cannot be negative.")]
        public short? UnitsOnOrder { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "Reorder level cannot be negative.")]
        public short? ReorderLevel { get; set; }

        public bool? Discontinued { get; set; }
    }
}
