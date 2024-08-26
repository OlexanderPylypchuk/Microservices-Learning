using AutoMapper;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Models.Dtos;

namespace ShoppingCartAPI
{
    public class MapperConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapping = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDTO>().ReverseMap();
            });
            return mapping;
        }
    }
}
