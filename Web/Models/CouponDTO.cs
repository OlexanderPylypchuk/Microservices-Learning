namespace Micro.Web.Models
{
	public class CouponDTO
	{
		public int Id { get; set; }
		public string CouponCode { get; set; }
		public int DiscountAmount { get; set; }
		public int MinAmount { get; set; }
	}
}
