//AutoMapperExtensions.cs
using AutoMapper;

namespace Aviva.PaymentOrders.Application.Adapters
{
    public static class AutoMapperExtensions
    {
        public static List<TDestination> MapList<TSource, TDestination>(this IMapper mapper, List<TSource> source)
        {
            return mapper.Map<List<TDestination>>(source);
        }

        public static TDestination MapObject<TSource, TDestination>(this IMapper mapper, TSource source)
        {
            return mapper.Map<TDestination>(source);
        }        
    }
}