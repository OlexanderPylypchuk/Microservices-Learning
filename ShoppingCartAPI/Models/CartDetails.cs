using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShoppingCartAPI.Models.Dtos;

namespace ShoppingCartAPI.Models
{
	public class CartDetails
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
