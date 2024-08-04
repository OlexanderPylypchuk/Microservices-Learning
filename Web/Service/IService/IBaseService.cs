using Micro.Web.Models;

namespace Micro.Web.Service.IService
{
	public interface IBaseService
	{
		Task<ResponceDTO?> SendAsync(RequestDTO requestDTO);
	}
}
