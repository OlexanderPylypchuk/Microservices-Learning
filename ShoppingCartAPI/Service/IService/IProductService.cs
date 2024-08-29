using ShoppingCartAPI.Models.Dtos;

namespace ShoppingCartAPI.Service.IService
{
	public interface IProductService
	{
		Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
	}
}
