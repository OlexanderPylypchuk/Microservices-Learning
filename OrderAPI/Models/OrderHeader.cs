namespace OrderAPI.Models
{
	public class OrderHeader
	{
		public int Id { get; set; }
		public string? UserId { get; set; }
		public string? CouponCode { get; set;}
		public double Discount { get; set; }
		public double OrderTotal { get; set; }
		public string? Name { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }
		public DateTime OrderTime { get; set; }
		public string? Status { get; set; }
		public IEnumerable<OrderDetails> OrderDetails { get; set; }
	}
}
