using AutoMapper;
using Ekart_Application.DTO;
using Ekart_Application.IServices;
using Ekart_Web_App.Models;
using Ekart_web_Application.DTO;

namespace Ekart_Application.EkartMapping
{
    public class EkartMapping1:Profile
    {
        public EkartMapping1()
        {
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>()
            .ForMember(dest => dest.Picture,  opt => opt.MapFrom(src => src.GetPictureBytes()));

            CreateMap<Category, CategoryCreateDto>()
                .ForMember(dest => dest.Picture,opt => opt.MapFrom(src => Convert.ToBase64String(src.Picture)));

            CreateMap<UpdateCategoryDto, Category>()
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            //Mapping for the supplier

            CreateMap<Supplier, SupplierDto>();
            CreateMap<UpdateSupplierDtoM, Supplier>();

            //mapping for get products
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, ResponseProductDto>();
            CreateMap<Product, ProductDtoM>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.CompanyName : null));

            CreateMap<CreateOrderDtoT, Order>().ReverseMap();
            CreateMap<Order, OrderDtoA>().ReverseMap();
            CreateMap<UpdateOrderDtoT, Order>().ReverseMap();
            CreateMap<UpdateOrderDetailDto, OrderDetail>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())  // Ignore OrderId, do not update it
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
                .ReverseMap();
            CreateMap<ProductByUnitStock, Product>().ReverseMap();
            CreateMap<CreateCustomerDtoT, Customer>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<Product, ProductDtoT>();
            //Order Mapping
            CreateMap<OrderDtoA, Order>().ReverseMap();

            //CreateMap<Order, OrderDto>()
            //.ForMember(dest => dest.RequireDate, opt => opt.MapFrom(src => src.RequiredDate))
            //.ForMember(dest => dest.ShipperDate, opt => opt.MapFrom(src => src.ShippedDate));
            CreateMap<UpdateOrderDtoA, Order>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //OrderDetail Mapping
            CreateMap<OrderDetail, OrderDetailsDtoA>().ReverseMap();
            CreateMap<OrderDetail, BillOrderDetailDto>()
         .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
         .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product.Category.CategoryName))
         .ForMember(dest => dest.PriceAfterDiscount, opt => opt.MapFrom(src =>
             src.UnitPrice * (decimal)src.Quantity * (1 - Convert.ToDecimal(src.Discount))));

            CreateMap<CountOrderDto, CountDaysResponseDto>()
                 .ForMember(dest => dest.DaysToShip, opt =>
                     opt.MapFrom(src => (src.OrderDate.HasValue && src.ShipDate.HasValue) ?
                         (src.ShipDate.Value - src.OrderDate.Value).Days : (int?)null));

            //Customer Mapping
            CreateMap<CustomerDto, Customer>().ReverseMap();
            //Autho Mapping
            CreateMap<CreateUserDto, User>().ReverseMap();

            //Mapping of Supplier
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<CreateSupplierDtoS, Supplier>().ReverseMap();
            CreateMap<Supplier, ResponseSupplierDtoS>().ReverseMap();
            //Map from CreateOrderDetailsDto to OrderDetailDto
            CreateMap<OrderDetail, ResponseOrderDetailDtoS>().ReverseMap();
            CreateMap<OrderDetail, CreateOrderDetailDtoS>().ReverseMap();
            //CreateMap<PatchProductDto, Product>();
            CreateMap<PatchProductDto, Product>();

            CreateMap<Product, ProductDtoS>().ReverseMap();

            CreateMap<Supplier, UpdateSupplierOwnDto>()
         .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
         .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => src.ContactName))
         .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
         .ForMember(dest => dest.ContactTitle, opt => opt.MapFrom(src => src.ContactTitle))
         .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
         .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
         .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region))
         .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
         .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
         .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
         .ForMember(dest => dest.Fax, opt => opt.MapFrom(src => src.Fax))
         .ForMember(dest => dest.HomePage, opt => opt.MapFrom(src => src.HomePage));

            // Mapping from UpdateSupplierOwnDto to Supplier entity (used for updates)
            CreateMap<UpdateSupplierOwnDto, Supplier>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => src.ContactName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ContactTitle, opt => opt.MapFrom(src => src.ContactTitle))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Fax, opt => opt.MapFrom(src => src.Fax))
                .ForMember(dest => dest.HomePage, opt => opt.MapFrom(src => src.HomePage));
            CreateMap<Supplier, ResponseSupplierDtoS>();
            CreateMap<CreateProductDto, Product>().ReverseMap();
            CreateMap<Product, ResponseProductDto>().ReverseMap();
            CreateMap<Customer, ResponseCustomerDtoN>().ReverseMap();
            CreateMap<Product, DiscontinuedProductDto>().ReverseMap();
            CreateMap<Product, ProductByUnitsOnOrderDto>().ReverseMap();
            CreateMap<PatchProductDto, Product>().ReverseMap();
            CreateMap<Customer, GetByRegionDto>()
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region))
                .ForMember(dest => dest.OrderCount, opt => opt.MapFrom(src => src.Orders.Count));
        }
    }
}
