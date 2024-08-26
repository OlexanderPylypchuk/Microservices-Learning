using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCartAPI.Models.Dtos
{
	public class CartHeaderDTO
	{
		[Key]
		public int Id { get; set; }
		public string? UserId { get; set; }
		public string? CouponCode { get; set; }
		[NotMapped]
		public double Discount { get; set; }
		[NotMapped]
		public double CartTotal { get; set; }
	}
}
