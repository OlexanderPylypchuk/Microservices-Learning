using ShoppingCartAPI.Models.Dto;

namespace ShoppingCartAPI.Service.IService
{
	public interface ICouponService
	{
		Task<CouponDTO> GetCoupon(string couponCode);
	}
}
