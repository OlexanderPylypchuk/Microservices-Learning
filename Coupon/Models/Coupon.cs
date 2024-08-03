using System.ComponentModel.DataAnnotations;

namespace CouponAPI.Models
{
	public class Coupon
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string CouponCode {  get; set; }
		[Required]
		public int DiscountAmount { get; set; }
		public int? MinAmount { get; set; }
	}
}
