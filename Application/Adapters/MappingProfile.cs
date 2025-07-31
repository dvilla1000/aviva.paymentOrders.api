using AutoMapper;
using Aviva.PaymentOrders.Domain.Entities;
using Aviva.PaymentOrders.Application.Adapters;

namespace Aviva.PaymentOrders.Application.Adapters
{   
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ReverseMap();
            CreateMap<PaymentOrder, OrderDTO>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.PaymentOrderDetails))
                .ReverseMap();

            CreateMap<PaymentOrderDetail, OrderDetailDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.Description))
                .ReverseMap();

            // CreateMap<PaymentOrderDetail, OrderDetailDTO>()
            //     .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            //     .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.Description))

            CreateMap<OrderDetailDTO, PaymentOrderDetail>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentOrder, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentOrderId, opt => opt.MapFrom(src => src.PaymentOrderId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
        }
    }
}