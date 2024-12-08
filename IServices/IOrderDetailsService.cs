using Ekart_web_Application.DTO;

namespace Ekart_Application.IServices
{
    public interface IOrderDetailsService
    {
        Task<IEnumerable<BillOrderDetailDto>> GetOrderBillDetailsAsync(int orderId);
        //GET: Get the bill amount for an OrderId 
        Task<Decimal> GetBillAmountAsync(int orderId);
        Task<string> AddOrderDetailAsync(CreateOrderDetailDtoS orderDetailDto);
        Task<IEnumerable<ResponseOrderDetailDtoS>> GetAllOrderDetailsAsync();
        Task UpdateOrderDetailAsync(int orderId, UpdateOrderDetailDto updateorderDetailDto);
    }
}
