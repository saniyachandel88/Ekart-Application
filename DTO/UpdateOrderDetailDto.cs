using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class UpdateOrderDetailDto
    {
        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be a positive number.")]
        public decimal UnitPrice { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Quantity must be a positive number greater than zero.")]
        public short Quantity { get; set; }

        [Range(0, 1, ErrorMessage = "Discount must be between 0 and 1.")]
        public float Discount { get; set; }
    }
}
