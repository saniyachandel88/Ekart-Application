namespace Ekart_web_Application.DTO
{
    public class ProductByUnitsOnOrderDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public int UnitsOnOrder { get; set; }
        public short? UnitsInStock { get; set; }
        public bool Discontinued { get; set; }
    }
}
