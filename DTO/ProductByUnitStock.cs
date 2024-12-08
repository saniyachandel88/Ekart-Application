namespace Ekart_web_Application.DTO
{
    public class ProductByUnitStock
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public int UnitsOnOrder { get; set; }
        public int UnitsInStock { get; set; }
    }
}
