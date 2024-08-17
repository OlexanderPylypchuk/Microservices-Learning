using Micro.Web.Models;
using Micro.Web.Service.IService;

namespace Micro.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {

            _baseService = baseService;

        }
        public async Task<ResponceDTO?> CreateAsync(ProductDTO productDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                Type = SD.ApiType.POST,
                Data = productDTO,
                Url = SD.ProductApiBase + "/api/product"
            });
        }

        public async Task<ResponceDTO?> DeleteAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                Type = SD.ApiType.DELETE,
                Url = SD.ProductApiBase + "/api/product/"+id
            });
        }

        public async Task<ResponceDTO?> GetAllAsync()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                Type = SD.ApiType.GET,
                Url = SD.ProductApiBase + "/api/product"
            });
        }

        public async Task<ResponceDTO?> GetAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                Type = SD.ApiType.GET,
                Url = SD.ProductApiBase + "/api/product/"+id
            });
        }

        public async Task<ResponceDTO?> UpdateAsync(ProductDTO productDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                Type = SD.ApiType.GET,
                Data = productDTO,
                Url = SD.ProductApiBase + "/api/product"
            });
        }
    }
}
