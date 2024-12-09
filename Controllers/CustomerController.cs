using Ekart_Application.IServices;
using Ekart_web_Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ekart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [Authorize(Roles = "admin")]
        [HttpGet("{city}")]
        public async Task<IActionResult> GetCustomerByCity(string city)
        {
            var customers = await _customerService.GetCustomerByCity(city);

            if (customers == null || !customers.Any())
            {
                return NotFound(new { message = "No customers found for the provided city." });
            }
            return Ok(customers);
        }

        [Authorize(Roles = "customer")]
        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CreateCustomerDtoT customer)
        {

            var result = await _customerService.AddCustomer(customer);
            return Ok(result);
        }
        [Authorize(Roles = "customer")]
        [HttpPatch("Edit/{customerId}")]
        public async Task<IActionResult> UpdateRegion(string customerId, [FromBody] PatchCustomerDtoT updateRegionDto)
        {
            if (string.IsNullOrEmpty(customerId) || updateRegionDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                await _customerService.UpdateCustomerRegionAsync(customerId, updateRegionDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            // Validate that the service is not null (edge case, usually unnecessary if DI is correct)
            if (_customerService == null)
            {
                return StatusCode(500, "Customer service is not available.");
            }


            var customers = await _customerService.GetAllCustomersAsync();
            if (customers == null || !customers.Any())
            {
                return NotFound("No customers found.");
            }

            // If everything is successful, return the customers
            return Ok(customers);
        }
        [HttpGet("orders/count")]
        public async Task<IActionResult> GetOrderCountByRegion([FromQuery] string region)
        {
            var result = await _customerService.GetOrderCountByRegionAsync(region);

            return Ok(result);
        }


       

        // Delete customer by ID
        [Authorize(Roles = "admin")]
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(string customerId)
        {
            try
            {
                // Call service to delete customer
                var result = await _customerService.DeleteCustomerAsync(customerId);

                // Return success message if customer is deleted
                return Ok($"Customer with ID {customerId} deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                // Return 404 Not Found if the customer is not found
                return NotFound(new { message = ex.Message });
            }

            catch (Exception ex)
            {
                // Log and handle any unexpected errors
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

    }
}
   

