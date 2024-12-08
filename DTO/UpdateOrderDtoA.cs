using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class UpdateOrderDtoA
    {
        public int? EmployeeId { get; set; }

        //[DataType(DataType.Date)]  
        public DateTime? OrderDate { get; set; }

        // [DataType(DataType.Date)]
        public DateTime? RequiredDate { get; set; }

        // [DataType(DataType.Date)]
        public DateTime? ShippedDate { get; set; }

        public int? ShipVia { get; set; }

        public decimal? Freight { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "ShipName must be between 5 and 100 characters.")]
        public string? ShipName { get; set; }

        [StringLength(200, MinimumLength = 5, ErrorMessage = "ShipAddress must be between 10 and 200 characters.")]
        public string? ShipAddress { get; set; }

        [StringLength(100, ErrorMessage = "ShipCity cannot exceed 100 characters.")]
        public string? ShipCity { get; set; }

        [StringLength(50, ErrorMessage = "ShipRegion cannot exceed 50 characters.")]
        public string? ShipRegion { get; set; }

        [StringLength(20, ErrorMessage = "ShipPostalCode cannot exceed 20 characters.")]
        public string? ShipPostalCode { get; set; }

        [StringLength(100, ErrorMessage = "ShipCountry cannot exceed 100 characters.")]
        public string? ShipCountry { get; set; }

    }
}
