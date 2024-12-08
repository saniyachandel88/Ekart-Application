using Ekart_web_Application.DTO;

namespace Ekart_Application.IServices
{
    public interface ISupplierService
    {
        Task<ResponseSupplierDtoS> CreateSupplier(CreateSupplierDtoS createSupplierDto);

        Task<List<ResponseSupplierDtoS>> GetAllSuppliers();
        Task<ResponseSupplierDtoS> ApproveOrRejectSupplier(int id, bool isApproved);
        Task<ResponseSupplierDtoS> UpdateSupplierOwnAsync(int supplierId, UpdateSupplierOwnDto updateSupplierOwnDto);
        Task<ResponseSupplierDtoS> GetSupplierDetailsAsync(int supplierId);

        Task<IEnumerable<SupplierDtoA>> GetSupplierById(int supplierId);

        Task UpdateSupplierAsync(int supplierId, UpdateSupplierDtoM updateSupplierDto);

        Task<IEnumerable<SupplierDto>> GetSuppliersByCountryAsync(string country);
    }
}
