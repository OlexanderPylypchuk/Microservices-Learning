using Micro.Web.Models;

namespace Micro.Web.Service.IService
{
	public interface ICouponService
	{
		Task<ResponceDTO?> GetCouponAsync(string code);
		Task<ResponceDTO?> GetAllCouponsAsync();
		Task<ResponceDTO?> GetCouponByIdAsync(int id);
		Task<ResponceDTO?> CreateAsync(CouponDTO couponDTO);
		Task<ResponceDTO?> UpdateAsync(CouponDTO couponDTO);
		Task<ResponceDTO?> DeleteAsync(int id);
	}
}
