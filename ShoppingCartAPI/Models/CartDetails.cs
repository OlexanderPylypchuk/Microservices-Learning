using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ShoppingCartAPI.Models.Dtos;

namespace ShoppingCartAPI.Models
{
	public class CartDetails
	{
		[Key]
		public int Id { get; set; }
		[ForeignKey(nameof(CartHeader))]
		public int CartHeaderId { get; set; }
		[ValidateNever]
		public CartHeader CartHeader { get; set; }
		public int ProductId { get; set; }
		[NotMapped]
		public ProductDTO ProductDto { get; set; }
		public int Count { get; set; }
	}
}
