using System.IdentityModel.Tokens.Jwt;
using Micro.Web.Models;
using Micro.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Micro.Web.Controllers
{
	public class CartController : Controller
	{
		private readonly IShoppingCartService _cartService;

        public CartController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await GetCartForLoggedUser());
        }

        public async Task<IActionResult> Remove(int id)
        {
			var responce = await _cartService.RemoveCart(id);
			if (responce != null && responce.Success)
			{
				return RedirectToAction(nameof(Index));
			}
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTO cartDTO)
        {
			var responce = await _cartService.AddCoupon(cartDTO);
			if (responce != null && responce.Success)
			{
				return RedirectToAction(nameof(Index));
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> RemoveCoupon(CartDTO cartDTO)
		{
			cartDTO.Header.CouponCode = "";
			var responce = await _cartService.AddCoupon(cartDTO);
			if (responce != null && responce.Success)
			{
				return RedirectToAction(nameof(Index));
			}
			return View();
		}

		private async Task<CartDTO> GetCartForLoggedUser()
        {
            var userId = User.Claims
				.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var responce = await _cartService.GetCart(userId);
            if (responce != null && responce.Success)
            {
                CartDTO cart = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(responce.Result));
                return cart;
            }
            return new CartDTO();
        }

		[HttpPost]
		public async Task<IActionResult> EmailCart()
		{
			var cart = await GetCartForLoggedUser();
			cart.Header.Email = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value;
			var responce = await _cartService.EmailCart(cart);
			if (responce != null && responce.Success)
			{
				TempData["success"] = "Email will be proccesed and sent shortly";
				return RedirectToAction(nameof(Index));
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SubmitOrder()
		{
			return View();
		}
	}
}
