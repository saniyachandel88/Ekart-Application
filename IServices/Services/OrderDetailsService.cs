using AutoMapper;
using Ekart_Web_App.Data;
using Ekart_Web_App.Models;
using Ekart_web_Application.DTO;
using Microsoft.EntityFrameworkCore;

namespace Ekart_Application.IServices.Services
{
    public class OrderDetailsService:IOrderDetailsService
    {
        private readonly EkartProjectContext _context;
        private readonly IMapper _mapper;

        public OrderDetailsService(EkartProjectContext ekartEcommerceDbContext, IMapper mapper)
        {
            _context = ekartEcommerceDbContext;
            _mapper = mapper;
        }

    
        public async Task<string> AddOrderDetailAsync(CreateOrderDetailDtoS orderDetailDto)
        {
            if (orderDetailDto == null)
            {
                throw new ArgumentNullException(nameof(orderDetailDto), "Order details cannot be null.");
            }

            // Ensure the associated Order exists in the database
            var order = await _context.Orders.FindAsync(orderDetailDto.OrderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {orderDetailDto.OrderId} does not exist.");
            }

            // Map the DTO to the entity and establish the foreign key relationship
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
            orderDetail.Order = order;

            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            return "Record Added Successfully";
        }

        public async Task<IEnumerable<ResponseOrderDetailDtoS>> GetAllOrderDetailsAsync()
        {
            var orderDetails = await _context.OrderDetails.Include(od => od.Product).ToListAsync();
            if (orderDetails == null || !orderDetails.Any())
            {
                throw new KeyNotFoundException("No order details found.");
            }

            return _mapper.Map<IEnumerable<ResponseOrderDetailDtoS>>(orderDetails);
        }


        public async Task<IEnumerable<BillOrderDetailDto>> GetOrderBillDetailsAsync(int orderId)
        {
            var orderDetails = await _context.OrderDetails
                .Include(od => od.Product)
                    .ThenInclude(p => p.Category)
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
            if (!orderDetails.Any())
            {
                throw new KeyNotFoundException($"Order with ID {orderId} was not found.");
            }

            return _mapper.Map<IEnumerable<BillOrderDetailDto>>(orderDetails);
        }
        public async Task<Decimal> GetBillAmountAsync(int orderId)
        {
            var orderDetailsExist = await _context.OrderDetails.AnyAsync(od => od.OrderId == orderId);
            if (!orderDetailsExist)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} was not found.");
            }
            return await _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .SumAsync(src =>
         src.UnitPrice * (decimal)src.Quantity * (1 - Convert.ToDecimal(src.Discount)));
        }

        public async Task UpdateOrderDetailAsync(int orderId, UpdateOrderDetailDto updateorderDetailDto)
        {

            var existingOrderDetail = await _context.OrderDetails
       .FirstOrDefaultAsync(od => od.OrderId == orderId);

            // If no matching order detail is found, throw a KeyNotFoundException
            if (existingOrderDetail == null)
            {
                throw new KeyNotFoundException("Order Detail not found.");
            }
            //Validate the input DTO
            if (updateorderDetailDto == null)
            {
                throw new ArgumentNullException(nameof(updateorderDetailDto), "Update order detail data cannot be null.");
            }


            _mapper.Map(updateorderDetailDto, existingOrderDetail);

            // Save the changes to the database
            var changes = await _context.SaveChangesAsync();

        }
    }
}
