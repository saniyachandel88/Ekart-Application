namespace Ekart_web_Application.DTO
{
    public class ResponseOrderDetailDtoS
    {
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? Quantity { get; set; }
        public decimal? Discount { get; set; }

    }
}
