using Micro.Web.Models;
using Micro.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Micro.Web.Controllers
{
	public class CouponController : Controller
	{
		private readonly ICouponService _couponService;
		public CouponController(ICouponService couponService)
		{
			_couponService = couponService;
		}

		public async Task<IActionResult> Index()
		{
			List<CouponDTO> list = new();
			ResponceDTO? responceDTO = await _couponService.GetAllCouponsAsync();
			if (responceDTO != null && responceDTO.Success)
			{
				list = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(responceDTO.Result));
			}
			return View(list);
		}
	}
}
