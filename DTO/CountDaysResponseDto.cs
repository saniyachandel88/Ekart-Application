namespace Ekart_web_Application.DTO
{
    public class CountDaysResponseDto
    {
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public int? DaysToShip { get; set; }

    }
}
