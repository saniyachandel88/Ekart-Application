using AutoMapper;
using Ekart_Application.DTO;
using Ekart_Web_App.Data;
using Ekart_Web_App.Models;
using Ekart_web_Application.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ekart_Application.IServices.Services
{
    public class ProductService:IProductService
    {
        private readonly EkartProjectContext _context;
        private readonly IMapper _mapper;
        public ProductService(EkartProjectContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;


        }

   
        public async Task PatchProductAsync(int productId, PatchProductDto patchProductDto)
        {
            // Fetch the product including its related entities if necessary
            var product = await _context.Products
                .Include(p => p.OrderDetails) // Include OrderDetails to check references if needed
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            // Handle changes to CategoryId
            if (patchProductDto.CategoryId.HasValue && patchProductDto.CategoryId.Value != product.CategoryId)
            {
                // Fetch the new Category
                var newCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == patchProductDto.CategoryId.Value);

                if (newCategory == null)
                {
                    throw new KeyNotFoundException($"Category with ID {patchProductDto.CategoryId} not found.");
                }

                // Update the CategoryId
                product.CategoryId = patchProductDto.CategoryId.Value;
            }

            // Map other fields from the DTO to the Product entity
            _mapper.Map(patchProductDto, product);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OutOfStockProductDto>> GetOutOfStockProductsAsync()
        {
            var outOfStocksProduct = await _context.Products.Where(p => p.UnitsInStock == 0).ToListAsync();
            return _mapper.Map<IEnumerable<OutOfStockProductDto>>(outOfStocksProduct);
        }
        public async Task UpdateProductAsync(int productId, PatchProductDto updateProductDto)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with {productId} not found");
            }
            _mapper.Map(updateProductDto, product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDtoT>> GetProductsBySupplierId(int supplierId)
        {
            // Validate input to ensure it is a positive value
            if (supplierId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(supplierId), "Supplier ID must be a positive integer.");
            }

            // Query the database for products associated with the specified supplier ID
            var products = await _context.Products
                .Where(p => p.SupplierId == supplierId)
                .ToListAsync();

            // Check if no products are found and throw an exception
            if (!products.Any())
            {
                throw new InvalidOperationException("No products found for the specified supplier ID.");
            }

            // Map and return the result
            return _mapper.Map<IEnumerable<ProductDtoT>>(products);
        }



        public async Task<IEnumerable<ProductByUnitStock>> GetProductsByUnitInStockAsync(int unitInStock)
        {
            // Validate input to ensure it fits within the range of Int16
            if (unitInStock < short.MinValue || unitInStock > short.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(unitInStock), "The value is out of range for UnitsInStock.");
            }

            // Convert to Int16 as required by the database
            short unitsInStock = (short)unitInStock;

            // Query the database for matching products
            var products = await _context.Products
                .Where(p => p.UnitsInStock == unitsInStock)
                .ToListAsync();

            // Check if no products are found and throw an exception
            if (!products.Any())
            {
                throw new InvalidOperationException("No products found with the specified units in stock.");
            }

            // Map and return the result
            return _mapper.Map<IEnumerable<ProductByUnitStock>>(products);
        }
        public async Task<int> AddProductAsync(CreateProductDto productDto)
        {
            // Check if the productDto is null
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto), "Product data cannot be null.");
            }

            // Validate category existence
            var categoryExists = await _context.Categories
                                               .AnyAsync(c => c.CategoryId == productDto.CategoryId);
            if (!categoryExists)
            {
                throw new KeyNotFoundException($"Category with ID {productDto.CategoryId} not found. Please verify the category ID.");
            }

            // Validate supplier existence
            var supplierExists = await _context.Suppliers
                                               .AnyAsync(s => s.SupplierId == productDto.SupplierId);
            if (!supplierExists)
            {
                throw new KeyNotFoundException($"Supplier with ID {productDto.SupplierId} not found. Please verify the supplier ID.");
            }

            // Validate quantityPerUnit
            if (string.IsNullOrWhiteSpace(productDto.QuantityPerUnit))
            {
                throw new ArgumentException("QuantityPerUnit cannot be null or empty. Please provide a valid quantity.");
            }

            // Validate unitPrice
            if (productDto.UnitPrice <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(productDto.UnitPrice), $"Unit price must be greater than zero.");
            }

            // Validate unitsInStock
            if (productDto.UnitsInStock < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(productDto.UnitsInStock), "Units in stock cannot be negative. Please provide a valid number.");
            }

            // Validate unitsOnOrder
            if (productDto.UnitsOnOrder < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(productDto.UnitsOnOrder), "Units on order cannot be negative. Please provide a valid number.");
            }

            // Validate reorderLevel
            if (productDto.ReorderLevel < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(productDto.ReorderLevel), "Reorder level cannot be negative. Please provide a valid number.");
            }

            // Validate discontinued (make sure it's a valid boolean)
            if (productDto.Discontinued != true && productDto.Discontinued != false)
            {
                throw new ArgumentException("Discontinued must be either true or false. Please provide a valid value.");
            }

            // Map DTO to Product entity
            var product = _mapper.Map<Product>(productDto);
            _context.Products.Add(product);

            // Save the product to the database
            var rowsAffected = await _context.SaveChangesAsync();
            if (rowsAffected <= 0)
            {
                throw new DbUpdateException("Failed to save the product in the database. Please try again.");
            }

            return product.ProductId; // Return the ID of the created product
        }

        public async Task<IEnumerable<ResponseProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products
                                          .Include(p => p.Supplier)   // Include Supplier if needed
                                          .Include(p => p.Category)   // Include Category if needed
                                          .ToListAsync();

            if (products == null || !products.Any())
            {
                throw new KeyNotFoundException("No products found. Please verify the request.");
            }

            return _mapper.Map<IEnumerable<ResponseProductDto>>(products);
        }

        public async Task<IEnumerable<ResponseProductDto>> GetAllProductsByCategoryIdAsync(int categoryId)
        {
            // Fetch products from the database
            var products = await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();

            // Map to DTOs using AutoMapper
            return _mapper.Map<IEnumerable<ResponseProductDto>>(products);
        }

        public async Task<IEnumerable<DiscontinuedProductDto>> GetDiscontinuedProductsAsync()
        {
            var discontinuedProducts = await _context.Products
                .Where(p => p.Discontinued == true)
                .ToListAsync();

            if (discontinuedProducts == null || !discontinuedProducts.Any())
            {
                throw new KeyNotFoundException("No discontinued products found. Please verify the request.");
            }

            return _mapper.Map<IEnumerable<DiscontinuedProductDto>>(discontinuedProducts);
        }

        public async Task<IEnumerable<ProductByUnitsOnOrderDto>> GetProductsByUnitsOnOrderAsync()
        {
            var products = await _context.Products
                                          .Where(p => p.UnitsOnOrder > 0)
                                          .ToListAsync();

            if (products == null || !products.Any())
            {
                throw new KeyNotFoundException("No products with units on order found. Please verify the request.");
            }

            return _mapper.Map<IEnumerable<ProductByUnitsOnOrderDto>>(products);
        }

        public async Task<ActionResult<decimal>> GetProductsAverageAsync()
        {
            try
            {
                var averageUnitPrice = await _context.Products
                    .Where(p => p.UnitPrice.HasValue)
                    .AverageAsync(p => p.UnitPrice.Value);

                return averageUnitPrice;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the operation
                throw new InvalidOperationException("Error calculating the average unit price.", ex);
            }
        }

        public async Task<IEnumerable<ProductDtoM>> GetProductsByCategoryNameAsync(string categoryName)
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .Where(p => p.Category != null && p.Category.CategoryName == categoryName)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<ProductDtoM>>(products);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching products for category '{categoryName}'.", ex);
            }
        }

        public async Task<IEnumerable<ProductDtoM>> GetProductsBySupplierNameAsync(string companyName)
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .Where(p => p.Supplier != null && p.Supplier.CompanyName == companyName)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<ProductDtoM>>(products);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching products for supplier '{companyName}'.", ex);
            }
        }

      
    }
}
