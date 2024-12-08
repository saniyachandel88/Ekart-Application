using Ekart_web_Application.DTO;

namespace Ekart_Application.IServices
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetCustomerByCity(string city);
        Task<string> AddCustomer(CreateCustomerDtoT customer);
        Task UpdateCustomerRegionAsync(string customerId, PatchCustomerDtoT patchCustomerDto);
        Task<IEnumerable<ResponseCustomerDtoN>> GetAllCustomersAsync();
        Task<GetByRegionDto> GetOrderCountByRegionAsync(string region);

        //Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<bool> DeleteCustomerAsync(string customerId);
    }
}
