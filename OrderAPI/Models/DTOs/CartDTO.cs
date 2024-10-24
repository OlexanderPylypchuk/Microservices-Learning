namespace OrderAPI.Models.DTOs
{
	public class CartDTO
	{
		public CartHeaderDTO Header { get; set; }
		public IEnumerable<CartDetailsDTO> Details { get; set; }
	}
}
