namespace Ekart_web_Application.DTO
{
    public class BillOrderDetailDto
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public decimal PriceAfterDiscount { get; set; }

    }
}
