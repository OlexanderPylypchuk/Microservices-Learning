using Micro.Web.Models;


namespace Micro.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponceDTO?> GetAllAsync();
        Task<ResponceDTO?> GetAsync(int id);
        Task<ResponceDTO?> CreateAsync(ProductDTO productDTO);
        Task<ResponceDTO?> UpdateAsync(ProductDTO productDTO);
        Task<ResponceDTO?> DeleteAsync(int id);
    }
}
