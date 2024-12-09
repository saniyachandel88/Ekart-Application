using AutoMapper;
using Ekart_Web_App.Data;
using Ekart_Web_App.Models;
using Ekart_web_Application.DTO;
using Microsoft.EntityFrameworkCore;

namespace Ekart_Application.IServices.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly EkartProjectContext _context;
        private readonly IMapper _mapper;
        public CustomerService(EkartProjectContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<IEnumerable<CustomerDto>> GetCustomerByCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentNullException(nameof(city), "City cannot be null or empty.");

            var customers = await _context.Customers
                .Where(c => c.City == city)
                .ToListAsync();

            if (!customers.Any())
                throw new InvalidOperationException($"No customers found in city '{city}'.");

            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }
        // Add a new customer
        public async Task<string> AddCustomer(CreateCustomerDtoT customerDto)
        {
            // Validate input
            if (customerDto == null)
            {
                throw new ArgumentNullException(nameof(customerDto), "Customer data cannot be null.");
            }

            // Map and add the customer to the context
            var customer = _mapper.Map<Customer>(customerDto);

            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customerDto), "Customer data cannot be null.");
            }

            // Generate CustomerId based on CompanyName
            if (string.IsNullOrEmpty(customer.CompanyName) || customer.CompanyName.Length < 3)
            {
                throw new ArgumentException("CompanyName must have at least 3 characters.");
            }

            customer.CustomerId = GenerateCustomerId(customer.CompanyName);

            // Add customer to the context
            _context.Customers.Add(customer);

            // Save changes to the database
            var changes = await _context.SaveChangesAsync();

            return "Record Created Successfully";
        }

        // Method to generate CustomerId
        private string GenerateCustomerId(string companyName)
        {
            // Get the first three letters of the CompanyName
            string prefix = companyName.Substring(0, 3).ToUpper();

            // Generate two random alphanumeric characters
            var random = new Random();
            string randomSuffix = new string(Enumerable.Range(0, 2)
                .Select(_ => (char)random.Next('A', 'Z' + 1))
            .ToArray());

            return prefix + randomSuffix;
        }
        public async Task UpdateCustomerRegionAsync(string customerId, PatchCustomerDtoT updateRegionDto)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentNullException(nameof(customerId), "Customer ID cannot be null or empty.");
            }

            if (updateRegionDto == null)
            {
                throw new ArgumentNullException(nameof(updateRegionDto), "Update data cannot be null.");
            }

            // Find the customer by ID
            var customer = await _context.Customers.FindAsync(customerId);

            // If the customer is not found, throw a KeyNotFoundException
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

            // Update the customer's region
            customer.Region = updateRegionDto.Region;

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ResponseCustomerDtoN>> GetAllCustomersAsync()
        {
            if (_context.Customers == null)
            {
                // Handle the case where the Customers table is null or not properly initialized.
                throw new InvalidOperationException("Customers table is not initialized.");
            }

            var customers = await _context.Customers.ToListAsync();

            if (customers == null || !customers.Any())
            {
                // Handle the case where no customers are found in the database.
                throw new InvalidOperationException("No customers found in the database.");
            }

            // Map the customers to ResponseCustomerDto and return.
            var responseCustomerDtos = _mapper.Map<IEnumerable<ResponseCustomerDtoN>>(customers);
            return responseCustomerDtos;
        }
        public async Task<GetByRegionDto> GetOrderCountByRegionAsync(string region)
        {

            var orderCount = await _context.Orders
        .Where(o => o.Customer.Region == region)
        .CountAsync();

            var result = new GetByRegionDto
            {
                Region = region,
                OrderCount = orderCount
            };

            return result;
        }

        public async Task<bool> DeleteCustomerAsync(string customerId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Step 1: Delete Order Details
                var orderIds = await _context.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Select(o => o.OrderId)
                    .ToListAsync();

                if (orderIds.Any())
                {
                    var orderDetails = await _context.OrderDetails
                        .Where(od => orderIds.Contains(od.OrderId))
                        .ToListAsync();
                    _context.OrderDetails.RemoveRange(orderDetails);
                }

                // Step 2: Delete Orders
                var orders = await _context.Orders
                    .Where(o => o.CustomerId == customerId)
                    .ToListAsync();
                _context.Orders.RemoveRange(orders);

                // Step 3: Delete Customer
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId);
                if (customer == null)
                {
                    return false; // Customer not found
                }
                _context.Customers.Remove(customer);

                // Save all changes
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Rollback transaction on error
                await transaction.RollbackAsync();
                Console.WriteLine($"Error during deletion: {ex.Message}");
                throw;
            }
        }
    }
}
