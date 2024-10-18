using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Micro.Web.Models;
using Micro.Web.Service.IService;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using System.Linq;

namespace Micro.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IProductService _productService;
		private readonly IShoppingCartService _shoppingCartService;
		public HomeController(ILogger<HomeController> logger, IProductService productService, IShoppingCartService shoppingCartService)
		{
			_logger = logger;
			_productService = productService;
			_shoppingCartService = shoppingCartService;
		}

		public async Task<IActionResult> Index()
		{
			var responce = await _productService.GetAllAsync();
			if(responce != null&&responce.Success)
			{
				var products = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(responce.Result));
				return View(products);
			}
			return View(new List<ProductDTO>());
		}
		[Authorize]
		public async Task<IActionResult> Details(int id)
		{
            var responce = await _productService.GetAsync(id);
            if (responce != null && responce.Success)
            {
                var product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(responce.Result));
				product.Count = 0;
                return View(product);
            }
            return RedirectToAction(nameof(Index));
		}
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Details(ProductDTO productDTO)
		{
			CartDTO cart = new CartDTO()
			{
				Header = new CartHeaderDTO()
				{
					UserId = User.Claims.FirstOrDefault(u => u.Type == JwtClaimTypes.Subject)?.Value
				}
			};
			CartDetailsDTO cartDetails = new CartDetailsDTO()
			{
				Count = productDTO.Count,
				ProductId = productDTO.Id
			};
			List<CartDetailsDTO> list = new List<CartDetailsDTO> { cartDetails };
			cart.Details = list;
			var responce = await _shoppingCartService.UpsertCart(cart);
			if (responce != null && responce.Success)
			{
				TempData["success"] = "Successfuly added item to your shopping cart";
				return RedirectToAction(nameof(Index));
			}
			TempData["error"] = responce.Message;
			return RedirectToAction(nameof(Details), productDTO.Id);
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
