using AutoMapper;
using OrderAPI.Models;
using OrderAPI.Models.DTOs;

namespace OrderAPI
{
    public class MapperConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapping = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeaderDTO, CartHeaderDTO>()
                    .ForMember(dest=> dest.CartTotal, u=>u.MapFrom(src=>src.OrderTotal))
                    .ReverseMap();

                config.CreateMap<CartDetailsDTO, OrderDetailsDTO>()
                    .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.ProductDto.Name))
                    .ForMember(dest => dest.ProductPrice, u => u.MapFrom(src => src.ProductDto.Price));

                config.CreateMap<OrderDetailsDTO, CartDetailsDTO>();

                config.CreateMap<OrderDetailsDTO, OrderDetails>().ReverseMap();
                config.CreateMap<OrderHeaderDTO, OrderHeader>().ReverseMap();
            });
            return mapping;
        }
    }
}
