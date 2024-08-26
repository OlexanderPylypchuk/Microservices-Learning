namespace ShoppingCartAPI.Models.Dtos
{
	public class CartDTO
	{
		public CartHeaderDTO Header { get; set; }
		public IEnumerable<CartDetailsDTO> Details { get; set; }
	}
}
