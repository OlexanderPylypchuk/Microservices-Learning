using OrderAPI.Models.DTOs;

namespace OrderAPI.Service.IService
{
	public interface IProductService
	{
		Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
	}
}
