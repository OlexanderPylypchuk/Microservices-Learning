using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using ShoppingCartAPI.Models.Dto;
using ShoppingCartAPI.Models.Dtos;
using ShoppingCartAPI.Service.IService;

namespace ShoppingCartAPI.Service
{
	public class ProductService : IProductService
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
		{
			var client = _httpClientFactory.CreateClient("Product");
			var responce = await client.GetAsync("api/product");
			var content = await responce.Content.ReadAsStringAsync();
			var resp = JsonConvert.DeserializeObject<ResponceDTO>(content);
			if(resp != null && resp.Success)
			{
				return JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(Convert.ToString(resp.Result));
			}
			return new List<ProductDTO>();
		}
	}
}
