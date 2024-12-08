using Ekart_Application.DTO;
using Ekart_web_Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Ekart_Application.IServices
{
    public interface IProductService
    {
        Task PatchProductAsync(int productId, PatchProductDto patchProductDto);
        Task<IEnumerable<OutOfStockProductDto>> GetOutOfStockProductsAsync();
        Task UpdateProductAsync(int productId, PatchProductDto updateProductDto);
        Task<IEnumerable<ProductByUnitStock>> GetProductsByUnitInStockAsync(int unitInStock);
        Task<IEnumerable<ProductDtoT>> GetProductsBySupplierId(int SupplierId);
        Task<int> AddProductAsync(CreateProductDto productDto);
        Task<IEnumerable<ResponseProductDto>> GetAllProductsAsync();
        Task<IEnumerable<DiscontinuedProductDto>> GetDiscontinuedProductsAsync();
        Task<IEnumerable<ProductByUnitsOnOrderDto>> GetProductsByUnitsOnOrderAsync();
        Task<IEnumerable<ResponseProductDto>> GetAllProductsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<ProductDtoM>> GetProductsByCategoryNameAsync(string categoryName);
        Task<IEnumerable<ProductDtoM>> GetProductsBySupplierNameAsync(string companyName);
        Task<ActionResult<decimal>> GetProductsAverageAsync();
    
    }
}
