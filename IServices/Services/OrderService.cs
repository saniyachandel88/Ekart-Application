using AutoMapper;
using Ekart_Web_App.Data;
using Ekart_Web_App.Models;
using Ekart_web_Application.DTO;
using Microsoft.EntityFrameworkCore;

namespace Ekart_Application.IServices.Services
{
    public class OrderService:IOrderService
    {
        private readonly EkartProjectContext _context;
        private readonly IMapper _mapper;
        //private readonly ILogger<CategoryService> _logger;

        public OrderService(EkartProjectContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            //_logger = logger;

        }
        public async Task UpdateOrderAsync(string? customerId, UpdateOrderDtoA updateOrderDto)
        {
            // Validate customerId
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentNullException(nameof(customerId), "Customer ID cannot be null or empty.");

            // Find the existing order
            var existingOrder = await _context.Orders
                .FirstOrDefaultAsync(o => o.CustomerId == customerId);

            if (existingOrder == null)
                throw new KeyNotFoundException($"Order for Customer ID '{customerId}' not found.");

            // Map the UpdateOrderDto to the existing Order
            _mapper.Map(updateOrderDto, existingOrder);

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<OrderDtoA>> GetOrdersByCustomerNameAsync(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentNullException(nameof(customerName), "Customer name cannot be null or empty.");

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Where(o => o.Customer.CompanyName == customerName)
                .ToListAsync();

            if (!orders.Any())
                throw new KeyNotFoundException($"No orders found for customer with name '{customerName}'.");

            return _mapper.Map<IEnumerable<OrderDtoA>>(orders);
        }
        public async Task<IEnumerable<OrderDtoA>> GetOrdersByDateAsync(DateTime orderDate)
        {
            var orders = await _context.Orders
                .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Date == orderDate.Date)
                .ToListAsync();

            if (!orders.Any())
                throw new InvalidOperationException($"No orders found on date '{orderDate:yyyy-MM-dd}'.");

            return _mapper.Map<IEnumerable<OrderDtoA>>(orders);
        }
        public async Task<IEnumerable<OrderDtoA>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();

            if (!orders.Any())
                throw new InvalidOperationException("No orders found in the system.");

            return _mapper.Map<IEnumerable<OrderDtoA>>(orders);
        }
        public async Task<CountDaysResponseDto> GetOrderWithDaysToShipAsync(CountOrderDto orderDto)
        {
            // Map CountOrderDto to CountOrderResponseDto using AutoMapper
            var responseDto = _mapper.Map<CountDaysResponseDto>(orderDto);

            if (orderDto.OrderDate.HasValue && orderDto.ShipDate.HasValue)
            {
                responseDto.DaysToShip = (orderDto.ShipDate.Value - orderDto.OrderDate.Value).Days;
            }
            // Return the response with the calculated DaysToShip
            return responseDto;
        }
        public async Task<string> AddOrders(CreateOrderDtoT orderDto)
        {
            try
            {
                var order = _mapper.Map<Order>(orderDto);
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return "Record Added Successfully";
            }
            catch (Exception ex)
            {
                return $"Error adding order: {ex.Message}";
            }
        }

        public async Task<IEnumerable<OrderDtoA>> GetOrdersBetweenDatesAsync(DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
            {
                throw new ArgumentException("From date cannot be later than To date");
            }
            var orders = await _context.Orders
              .Where(o => o.OrderDate.HasValue &&
                  o.OrderDate.Value.Date >= fromDate.Date &&
                  o.OrderDate.Value.Date <= toDate.Date)
              .ToListAsync();

            return _mapper.Map<IEnumerable<OrderDtoA>>(orders);
        }


        public async Task PartialUpdateOrderAsync(string? customerId, UpdateOrderDtoT updateOrderDto)
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.CustomerId == customerId);

            if (existingOrder == null)
                // Instead of a generic exception, throw a KeyNotFoundException
                throw new KeyNotFoundException("Order not found");

            if (updateOrderDto.ShipName != null)
                existingOrder.ShipName = updateOrderDto.ShipName;
            if (updateOrderDto.ShipAddress != null)
                existingOrder.ShipAddress = updateOrderDto.ShipAddress;

            await _context.SaveChangesAsync();
        }

        public async Task<string> GetCustomerWithHighestOrderAsync()
        {

            if (_context.Orders == null)
            {
                throw new InvalidOperationException("Orders table is not initialized.");
            }

            var customerWithHighestOrder = await _context.Orders
                .GroupBy(o => o.CustomerId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            if (customerWithHighestOrder == null)
            {
                throw new KeyNotFoundException("No orders found for any customer.");
            }

            return customerWithHighestOrder;


        }
    }
}
