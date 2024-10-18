namespace Micro.Web.Models
{
	public class CartDTO
	{
		public CartHeaderDTO Header { get; set; }
		public IEnumerable<CartDetailsDTO> Details { get; set; }
	}
}
