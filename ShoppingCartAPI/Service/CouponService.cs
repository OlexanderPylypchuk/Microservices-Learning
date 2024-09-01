using Newtonsoft.Json;
using ShoppingCartAPI.Models.Dto;
using ShoppingCartAPI.Service.IService;

namespace ShoppingCartAPI.Service
{
	public class CouponService : ICouponService
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
			_httpClientFactory = httpClientFactory;            
        }
        public async Task<CouponDTO> GetCoupon(string couponCode)
		{
			var client = _httpClientFactory.CreateClient("Coupon");
			var responce = await client.GetAsync("api/coupon/GetByCode/"+couponCode);
			var content = await responce.Content.ReadAsStringAsync();
			var responceDto = JsonConvert.DeserializeObject<ResponceDTO>(content);
			if (responceDto != null && responceDto.Success)
			{
				return JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(responceDto.Result));
			}
			return new CouponDTO();
		}
	}
}
