using Ekart_Application.IServices;
using Ekart_web_Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ekart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsService _orderDetailsService;
        public OrderDetailsController(IOrderDetailsService orderDetailsService)
        {
            _orderDetailsService = orderDetailsService;
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddOrderDetail([FromBody] CreateOrderDetailDtoS createOrderDetailDto)
        {
            var result = await _orderDetailsService.AddOrderDetailAsync(createOrderDetailDto);
            return Ok(result);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetails()
        {
            var orderDetails = await _orderDetailsService.GetAllOrderDetailsAsync();
            return Ok(orderDetails);
        }
        [Authorize(Roles = "customer")]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderBillDetails(int orderId)
        {
            var billDetails = await _orderDetailsService.GetOrderBillDetailsAsync(orderId);
            return Ok(billDetails);
        }
        [Authorize(Roles = "customer")]
        [HttpGet("{orderId}/bill-amount")]
        public async Task<decimal> GetBillAmountAsync(int orderId)
        {
            return await _orderDetailsService.GetBillAmountAsync(orderId);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("edit/{orderId}")]
        public async Task<IActionResult> UpdateOrderDetail(int orderId, [FromBody] UpdateOrderDetailDto updateorderDetailDto)
        {
            try
            {
                await _orderDetailsService.UpdateOrderDetailAsync(orderId, updateorderDetailDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
