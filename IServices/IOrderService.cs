using Ekart_web_Application.DTO;

namespace Ekart_Application.IServices
{
    public interface IOrderService
    {
        Task UpdateOrderAsync(string? customerId, UpdateOrderDtoA updateorderDto);

        // GET: Display Orders by Customer Name
        Task<IEnumerable<OrderDtoA>> GetOrdersByCustomerNameAsync(string customerName);

        // GET: Search Order by OrderDate
        Task<IEnumerable<OrderDtoA>> GetOrdersByDateAsync(DateTime orderDate);
        //Task<IEnumerable<OrderDto>> GetOrdersByDateAsync(string orderDate);

        //Get: Get All Orders 
        Task<IEnumerable<OrderDtoA>> GetAllOrdersAsync();
        Task<CountDaysResponseDto> GetOrderWithDaysToShipAsync(CountOrderDto orderDto);
        Task<string> AddOrders(CreateOrderDtoT orderDto);
        Task PartialUpdateOrderAsync(string? customerId, UpdateOrderDtoT updateOrderDto);
        Task<IEnumerable<OrderDtoA>> GetOrdersBetweenDatesAsync(DateTime fromDate, DateTime toDate);
        Task<string> GetCustomerWithHighestOrderAsync();
        IEnumerable<object> GetUniqueProductsOrderedByCustomer(string customerId);

    }
}
