using Micro.Web.Models;

namespace Micro.Web.Service.IService
{
	public interface IShoppingCartService
	{
		Task<ResponceDTO> GetCart(string userId);
		Task<ResponceDTO> UpsertCart(CartDTO cartDTO);
		Task<ResponceDTO> RemoveCart(int detailsId);
		Task<ResponceDTO> AddCoupon(CartDTO cartDTO);
		Task<ResponceDTO> EmailCart(CartDTO cartDTO);
	}
}
