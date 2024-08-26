using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCartAPI.Models.Dtos
{
	public class CartDetailsDTO
	{
		[Key]
		public int Id { get; set; }
		public int CartHeaderId { get; set; }
		[ForeignKey(nameof(CartHeaderId))]
		public CartHeader CartHeader { get; set; }
		public int ProductId { get; set; }
		[NotMapped]
		public ProductDTO ProductDto { get; set; }
		public int Count { get; set; }
	}
}
