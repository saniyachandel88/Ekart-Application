using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class CountOrderDto
    {
        [Required(ErrorMessage = "Order date is required.")]
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "Ship date is required.")]
        public DateTime? ShipDate { get; set; }
    }
}
