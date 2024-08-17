using AutoMapper;
using ProductAPI.Models;
using ProductAPI.Models.Dtos;

namespace ProductAPI
{
    public class MapperConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapping = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDTO>().ReverseMap();
            });
            return mapping;
        }
    }
}
