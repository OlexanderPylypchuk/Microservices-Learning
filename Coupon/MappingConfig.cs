using AutoMapper;
using CouponAPI.Models;
using CouponAPI.Models.Dto;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CouponAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mapping = new MapperConfiguration(config=>
			{
				config.CreateMap<Coupon,CouponDTO>().ReverseMap();
			});
			return mapping;
		}
	}
}
