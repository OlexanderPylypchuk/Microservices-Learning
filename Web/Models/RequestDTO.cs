using Micro.Web;
using static Micro.Web.SD;
namespace Micro.Web.Models
{
	public class RequestDTO
	{
		public ApiType Type { get; set; }
		public string Url { get; set; }
		public object? Data { get; set; }
		public string AccessToken { get; set; }
	}
}
