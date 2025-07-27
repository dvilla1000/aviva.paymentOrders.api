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
                .ReverseMap();
        }
    }
}