using Micro.Web.Models;
using Micro.Web.Service.IService;

namespace Micro.Web.Service
{
	public class CouponService : ICouponService
	{
		private readonly IBaseService _baseService;
		public CouponService(IBaseService baseService)
		{
			_baseService = baseService;
		}

		public async Task<ResponceDTO?> CreateAsync(CouponDTO couponDTO)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				Type = SD.ApiType.POST,
				Url = SD.CouponApiBase + "/api/coupon",
				Data = couponDTO
			});
		}

		public async Task<ResponceDTO?> DeleteAsync(int id)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				Type = SD.ApiType.DELETE,
				Url = SD.CouponApiBase + "/api/coupon/" + id
			});
		}

		public async Task<ResponceDTO?> GetAllCouponsAsync()
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				Type = SD.ApiType.GET,
				Url = SD.CouponApiBase + "/api/coupon"
			});
		}

		public async Task<ResponceDTO?> GetCouponAsync(string code)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				Type = SD.ApiType.GET,
				Url = SD.CouponApiBase + "/api/coupon/GetByCode/" + code
			});
		}

		public async Task<ResponceDTO?> GetCouponByIdAsync(int id)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				Type = SD.ApiType.GET,
				Url = SD.CouponApiBase + "/api/coupon/" + id
			});
		}

		public async Task<ResponceDTO?> UpdateAsync(CouponDTO couponDTO)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				Type = SD.ApiType.PUT,
				Url = SD.CouponApiBase + "/api/coupon",
				Data = couponDTO
			});
		}
	}
}
