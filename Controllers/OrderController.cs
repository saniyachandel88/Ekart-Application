using Ekart_Application.IServices;
using Ekart_web_Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ekart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize(Roles = "admin")]
        [HttpPut("edit/{customerId}")]
        public async Task<IActionResult> UpdateOrder(string? customerId, [FromBody] UpdateOrderDtoA updateorderDto)
        {
            await _orderService.UpdateOrderAsync(customerId, updateorderDto);
            return NoContent();
        }
        [Authorize(Roles = "admin")]
        [HttpGet("CustomerName/{customerName}")]
        public async Task<IActionResult> GetOrdersByCustomerName(string customerName)
        {
            var orders = await _orderService.GetOrdersByCustomerNameAsync(customerName);
            return Ok(orders);
        }
        [Authorize(Roles = "admin")]
        [HttpGet("OrderDate/{orderDate}")]
        public async Task<IActionResult> GetOrdersByDate([FromRoute] string orderDate)
        {
            // Parse the string date into DateTime and remove the time part
            if (DateTime.TryParse(orderDate, out DateTime parsedDate))
            {
                // Call the service to get orders by date
                var orders = await _orderService.GetOrdersByDateAsync(parsedDate.Date); // Use .Date to ignore time

                // Return 404 if no orders are found
                if (orders == null || !orders.Any())
                {
                    return NotFound("No orders found for the specified date.");
                }

                return Ok(orders);
            }
            else
            {
                return BadRequest("Invalid date format. Please use yyyy-MM-dd.");
            }
        }
        [Authorize(Roles ="admin")]
        [HttpGet]
        public async Task<IEnumerable<OrderDtoA>> GetAllOrdersAsync()
        {
            return await _orderService.GetAllOrdersAsync();
        }

        [HttpPost("calculate-days")]
        public async Task<ActionResult<CountDaysResponseDto>> CalculateDaysToShipAsync([FromBody] CountOrderDto orderDto)
        {
            if (!orderDto.OrderDate.HasValue || !orderDto.ShipDate.HasValue)
            {
                return BadRequest("Both OrderDate and ShipDate are required.");
            }

            var response = await _orderService.GetOrderWithDaysToShipAsync(orderDto);
            return Ok(response);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] CreateOrderDtoT orderDto)
        {
            var result = await _orderService.AddOrders(orderDto);
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("edit/{customerId}")]
        public async Task<IActionResult> PartialUpdateOrder(string customerId, [FromBody] UpdateOrderDtoT updateOrderDto)
        {
            await _orderService.PartialUpdateOrderAsync(customerId, updateOrderDto);
            return NoContent();
        }
        [Authorize(Roles = "customer")]
        [HttpGet("BetweenDate/{fromDate}/{toDate}")]
        public async Task<IActionResult> GetOrdersBetweenDates(DateTime fromDate, DateTime toDate)
        {
            var orders = await _orderService.GetOrdersBetweenDatesAsync(fromDate, toDate);
            return Ok(orders);
        }
        [Authorize(Roles = "admin")]
        [HttpGet("highest-order")]
        public async Task<IActionResult> GetCustomerWithHighestOrder()
        {
            var customerId = await _orderService.GetCustomerWithHighestOrderAsync();
            if (string.IsNullOrEmpty(customerId))
            {
                return NotFound("No customer found with the highest number of orders.");
            }

            return Ok(new { CustomerId = customerId });
        }
    }
}
