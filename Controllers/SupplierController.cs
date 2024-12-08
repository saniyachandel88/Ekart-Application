using Ekart_Application.IServices;
using Ekart_web_Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ekart_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {

        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        [Authorize(Roles = "admin")]
        [HttpPost("Add")]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierDtoS createSupplierDto)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors to the client
                return BadRequest(ModelState);
            }

            var response = await _supplierService.CreateSupplier(createSupplierDto);
            // Return the success response with the message
            return Ok("Record Added Successfully!!");
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliers();
            return Ok(suppliers);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("ApproveOrReject/{id}")]
        public async Task<IActionResult> ApproveOrRejectSupplier(int id, [FromQuery] bool isApproved)
        {
            try
            {
                // Call the service to approve or reject the supplier
                var updatedSupplier = await _supplierService.ApproveOrRejectSupplier(id, isApproved);

                // Return a success response
                return Ok(new { message = $"Supplier {(isApproved ? "approved" : "rejected")} successfully.", updatedSupplier });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [Authorize(Roles = "supplier")]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateSupplierOwnDto updateSupplierOwnDto)
        {
            // Extract SupplierId from the token
            var supplierId = GetSupplierIdFromToken();

            if (supplierId == null)
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            // Call the service to update the supplier's profile using the extracted SupplierId
            var updatedSupplier = await _supplierService.UpdateSupplierOwnAsync(supplierId.Value, updateSupplierOwnDto);

            if (updatedSupplier == null)
            {
                return NotFound(new { message = "Supplier not found." });
            }

            return Ok(updatedSupplier);  // Return the updated supplier details
        }
        [HttpGet("profile")]
        [Authorize(Roles = "supplier")]
        public async Task<IActionResult> GetSupplierProfile()
        {
            try
            {
                // Extract the Supplier ID from the token
                var supplierIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id"); // Replace "Id" with the correct claim name
                if (supplierIdClaim == null)
                {
                    return Unauthorized(new { message = "Invalid token: Supplier ID not found." });
                }

                var supplierId = int.Parse(supplierIdClaim.Value);

                // Get supplier details
                var supplierDetails = await _supplierService.GetSupplierDetailsAsync(supplierId);
                return Ok(supplierDetails);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }
        private int? GetSupplierIdFromToken()
        {
            // Extract the 'SupplierId' claim from the token
            var supplierIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            // If the claim exists and can be parsed to an integer, return it
            if (supplierIdClaim != null && int.TryParse(supplierIdClaim.Value, out var supplierId))
            {
                return supplierId;
            }

            // If not, return null (indicating an invalid token or missing claim)
            return null;
        }
        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            // Call the service to get the supplier by ID
            var suppliers = await _supplierService.GetSupplierById(id);

            // Check if the result is empty
            if (suppliers == null || !suppliers.Any())
            {
                return NotFound(new { message = $"Supplier with ID {id} not found." });
            }

            // Return the result as an OkObjectResult
            return Ok(suppliers);
        }
        [Authorize(Roles = "admin")]
        // Update Supplier
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierDtoM updateSupplierDto)
        {
            // Call the service to update supplier
            await _supplierService.UpdateSupplierAsync(id, updateSupplierDto);

            return NoContent(); // No content on successful update
        }

        [Authorize(Roles = "admin")]
     
        [HttpGet("by-country/{country}")]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSuppliersByCountry(string country)
        {
            // Call the service to get suppliers by country
            var suppliers = await _supplierService.GetSuppliersByCountryAsync(country);

            // If no suppliers found, let the global exception handler deal with it
            if (suppliers == null || !suppliers.Any())
            {

                throw new KeyNotFoundException($"No suppliers found for the country: {country}");
            }

            return Ok(suppliers); // Return suppliers if found
        }
    }
}
