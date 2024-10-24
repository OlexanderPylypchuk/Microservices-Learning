using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.DTOs
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
		public string? Email { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? PhoneNumber { get; set; }
	}
}
