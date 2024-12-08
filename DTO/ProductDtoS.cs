using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class ProductDtoS
    {
        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid Supplier ID.")]
        public int? SupplierId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid Category ID.")]
        public int? CategoryId { get; set; }

        [StringLength(100, ErrorMessage = "Quantity per unit cannot exceed 100 characters.")]
        public string QuantityPerUnit { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
        public decimal? UnitPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Units in stock cannot be negative.")]
        public int UnitsInStock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Units on order cannot be negative.")]
        public int? UnitsOnOrder { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Reorder level cannot be negative.")]
        public int? ReorderLevel { get; set; }

        public bool? Discontinued { get; set; }
    }
}
