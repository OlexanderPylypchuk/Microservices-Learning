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
		private readonly IOrderService _orderService;

        public CartController(IShoppingCartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
			_orderService = orderService;
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

		[Authorize]
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

		[Authorize]
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

		[Authorize]
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

		[Authorize]
		public async Task<IActionResult> Checkout()
		{
			var CartDto = await GetCartForLoggedUser();
			return View(CartDto);
		}

		[HttpPost]
		[ActionName("Checkout")]
		public async Task<IActionResult> Checkout(CartDTO cartDTO)
		{
			CartDTO cart = await GetCartForLoggedUser();
			cart.Header.Email = cartDTO.Header.Email;
			cart.Header.PhoneNumber = cartDTO.Header.PhoneNumber;
			cart.Header.FirstName = cartDTO.Header.FirstName;
			cart.Header.LastName = cartDTO.Header.LastName;

			ResponceDTO responce = await _orderService.CreateOrderAsync(cart);

			OrderHeaderDTO orderHeaderDTO = JsonConvert.DeserializeObject<OrderHeaderDTO>(Convert.ToString(responce.Result));

			if(responce != null && responce.Success)
			{
				//Тут має бути спосіб оплати, але я не відкрив ФОП і тому нічого тут не буде XD
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
