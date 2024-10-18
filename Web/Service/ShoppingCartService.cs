using Micro.Web.Models;
using Micro.Web.Service.IService;

namespace Micro.Web.Service
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly IBaseService _baseService;
        public ShoppingCartService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponceDTO> AddCoupon(CartDTO cartDTO)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				Type = SD.ApiType.POST,
				Url = SD.ShoppingCartApiBase + "/api/cart/applycoupon",
				Data = cartDTO
			});
		}

		public async Task<ResponceDTO> EmailCart(CartDTO cartDTO)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				Type = SD.ApiType.POST,
				Url = SD.ShoppingCartApiBase + "/api/cart/emailcartrequest",
				Data = cartDTO
			});
		}

		public async Task<ResponceDTO> GetCart(string userId)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				Type = SD.ApiType.GET,
				Url = SD.ShoppingCartApiBase + "/api/cart/getcart/" + userId,
			});
		}

		public async Task<ResponceDTO> RemoveCart(int detailsId)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				Type = SD.ApiType.POST,
				Url = SD.ShoppingCartApiBase + "/api/cart/removecart",
				Data = detailsId
			});
		}

		public async Task<ResponceDTO> UpsertCart(CartDTO cartDTO)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				Type = SD.ApiType.POST,
				Url = SD.ShoppingCartApiBase + "/api/cart/cartupsert",
				Data = cartDTO
			});
		}
	}
}
