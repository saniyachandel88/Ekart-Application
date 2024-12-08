using AutoMapper;
using Ekart_Web_App.Data;
using Ekart_Web_App.Models;
using Ekart_web_Application.DTO;
using Microsoft.EntityFrameworkCore;

namespace Ekart_Application.IServices.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly EkartProjectContext _context;
        private readonly IMapper _mapper;

        public SupplierService(EkartProjectContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseSupplierDtoS> ApproveOrRejectSupplier(int id, bool isApproved)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                throw new KeyNotFoundException($"No supplier found with ID {id}.");
            }

            // Update the IsApproved status
            supplier.IsApproved = isApproved;

            // Save changes
            await _context.SaveChangesAsync();

            // Return the updated supplier
            return _mapper.Map<ResponseSupplierDtoS>(supplier);
        }

        public async Task<ResponseSupplierDtoS> CreateSupplier(CreateSupplierDtoS createSupplierDto)
        {

            if (createSupplierDto == null)
            {
                throw new ArgumentNullException(nameof(createSupplierDto), "Supplier Creation data cannot be null");
            }

            var supplier = _mapper.Map<Supplier>(createSupplierDto);
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
            return _mapper.Map<ResponseSupplierDtoS>(supplier);
        }

        public async Task<List<ResponseSupplierDtoS>> GetAllSuppliers()
        {
            var suppliers = await _context.Suppliers.ToListAsync();
            if (!suppliers.Any())
            {
                throw new KeyNotFoundException("No suppliers found in the database.");
            }

            // Map the list of suppliers to a list of response DTOs and return
            return _mapper.Map<List<ResponseSupplierDtoS>>(suppliers);
        }

        public async  Task<IEnumerable<SupplierDtoA>> GetSupplierById(int supplierId)
        {
            if (supplierId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(supplierId), "Supplier ID must be greater than 0.");
            }

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == supplierId);

            if (supplier == null)
            {
                throw new KeyNotFoundException($"Supplier with ID {supplierId} was not found.");
            }

            // Map the supplier to a SupplierDto and return as a collection
            return new List<SupplierDtoA> { _mapper.Map<SupplierDtoA>(supplier) };
        }

        public async  Task<ResponseSupplierDtoS> GetSupplierDetailsAsync(int supplierId)
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId);
            if (supplier == null)
            {
                throw new KeyNotFoundException("Supplier not found.");
            }

            // Use AutoMapper to map supplier entity to ResponseSupplierDto
            var supplierDto = _mapper.Map<ResponseSupplierDtoS>(supplier);
            return supplierDto;
        }

        public async Task<IEnumerable<SupplierDto>> GetSuppliersByCountryAsync(string country)
        {
            try
            {
                var suppliers = await _context.Suppliers
                    .Where(s => s.Country == country)
                    .Select(s => new SupplierDto
                    {
                        SupplierId = s.SupplierId,
                        CompanyName = s.CompanyName,
                        ContactName = s.ContactName,
                        ContactTitle = s.ContactTitle,
                        Address = s.Address,
                        City = s.City,
                        Region = s.Region,
                        PostalCode = s.PostalCode,
                        Country = s.Country,
                        Phone = s.Phone,
                        Fax = s.Fax
                    })
                    .ToListAsync();

                return suppliers;
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("An error occurred while retrieving suppliers.", ex);
            }
        }

        public async  Task UpdateSupplierAsync(int supplierId, UpdateSupplierDtoM updateSupplierDto)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(supplierId);

                if (supplier == null)
                {
                    throw new KeyNotFoundException($"Supplier with ID {supplierId} not found.");
                }

                // Mapping updated values from the DTO to the existing supplier
                _mapper.Map(updateSupplierDto, supplier);
                await _context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the supplier.", ex);
            }
        }

        public  async Task<ResponseSupplierDtoS> UpdateSupplierOwnAsync(int supplierId, UpdateSupplierOwnDto updateSupplierOwnDto)
        {
            var supplier = await _context.Suppliers
               .FirstOrDefaultAsync(s => s.SupplierId == supplierId);

            if (supplier == null)
            {
                return null; // Supplier not found
            }

            // Map the updated details to the supplier entity
            _mapper.Map(updateSupplierOwnDto, supplier);

            // Save the changes
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();

            // Return the updated supplier details
            return _mapper.Map<ResponseSupplierDtoS>(supplier);
        }
    }
}
