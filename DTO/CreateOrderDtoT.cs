using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class CreateOrderDtoT
    {

        [Required(ErrorMessage = "Customer ID is required.")]
        public string? CustomerId { get; set; }

        [Required(ErrorMessage = "Employee ID is required.")]
        public int? EmployeeId { get; set; }

        [Required(ErrorMessage = "Order Date is required.")]
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "Required Date is required.")]
        public DateTime? RequiredDate { get; set; }

        // Optional: Ensure the ShippedDate is only valid after RequiredDate if it exists
        [Required(ErrorMessage = "Shipped Date cannot be earlier than the Required Date.")]
        public DateTime? ShippedDate { get; set; }

        [Required(ErrorMessage = "Ship Via is required.")]
        public int? ShipVia { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Freight cannot be negative.")]
        public decimal? Freight { get; set; }

        [StringLength(100, ErrorMessage = "Ship Name cannot be longer than 100 characters.")]
        public string? ShipName { get; set; }

        [StringLength(200, ErrorMessage = "Ship Address cannot be longer than 200 characters.")]
        public string? ShipAddress { get; set; }

        [StringLength(100, ErrorMessage = "Ship City cannot be longer than 100 characters.")]
        public string? ShipCity { get; set; }

        [StringLength(100, ErrorMessage = "Ship Region cannot be longer than 100 characters.")]
        public string? ShipRegion { get; set; }

        [StringLength(20, ErrorMessage = "Ship Postal Code cannot be longer than 20 characters.")]
        public string? ShipPostalCode { get; set; }

        [StringLength(50, ErrorMessage = "Ship Country cannot be longer than 50 characters.")]
        public string? ShipCountry { get; set; }
    }
}
