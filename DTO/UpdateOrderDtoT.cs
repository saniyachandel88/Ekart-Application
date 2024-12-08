using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class UpdateOrderDtoT
    {
        [Required(ErrorMessage = "Ship Name is required.")]

        public string? ShipName { get; set; }

        [Required(ErrorMessage = "Ship Address is required.")]

        public string? ShipAddress { get; set; }
    }
}
