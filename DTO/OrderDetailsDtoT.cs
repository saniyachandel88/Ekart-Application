using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class OrderDetailsDtoT
    {
        [Required(ErrorMessage = "ProductId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive integer.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "UnitPrice is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "UnitPrice must be a positive value greater than zero.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, short.MaxValue, ErrorMessage = "Quantity must be a positive integer.")]
        public short Quantity { get; set; }

        [Range(0.0, 1.0, ErrorMessage = "Discount must be between 0 and 1 (representing 0% to 100%).")]
        public float Discount { get; set; }

    }
}
