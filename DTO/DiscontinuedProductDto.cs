namespace Ekart_web_Application.DTO
{
    public class DiscontinuedProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public bool? Discontinued { get; set; }

    }
}
