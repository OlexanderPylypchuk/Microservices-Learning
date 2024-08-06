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
			else
			{
				TempData["error"] = responceDTO.Message;
			}
			return View(list);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CouponDTO couponDTO)
		{
			if (ModelState.IsValid)
			{
				ResponceDTO? responceDTO = await _couponService.CreateAsync(couponDTO);
				if (responceDTO != null && !responceDTO.Success)
					TempData["error"] = responceDTO.Message;
				else
					TempData["success"] = "Created successfuly";
			}
			else
			{
				TempData["error"] = "Something wrong with model";
			}
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Update(int id)
		{
			CouponDTO obj;
			ResponceDTO? responceDTO = await _couponService.GetCouponByIdAsync(id);
			if (responceDTO != null && responceDTO.Success)
			{
				obj = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(responceDTO.Result));
				return View(obj);
			}
			else
			{
				TempData["error"] = responceDTO.Message;
			}
			return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> Update(CouponDTO couponDTO)
		{
			if (ModelState.IsValid)
			{
				ResponceDTO? responceDTO = await _couponService.UpdateAsync(couponDTO);
				if(responceDTO != null && !responceDTO.Success)
					TempData["error"] = responceDTO.Message;
				else
					TempData["success"] = "Updated successfuly";
			}
			else
			{
				TempData["error"] = "Something wrong with model";
			}
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Delete(int id)
		{
			ResponceDTO? responceDTO = await _couponService.DeleteAsync(id);
			TempData["success"] = "Deleted successfuly";
			return RedirectToAction("Index");
		}
	}
}
