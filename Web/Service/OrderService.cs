using Micro.Web.Models;
using Micro.Web.Service.IService;

namespace Micro.Web.Service
{
	public class OrderService : IOrderService
	{
		private readonly IBaseService _baseService;

		public OrderService(IBaseService baseService)
		{
			_baseService = baseService;
		}

		public async Task<ResponceDTO?> CreateOrderAsync(CartDTO cartDTO)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				Data = cartDTO,
				Type = SD.ApiType.POST,
				Url = SD.OrderApiBase + "/api/order/CreateOrder"
			});
		}
	}
}
