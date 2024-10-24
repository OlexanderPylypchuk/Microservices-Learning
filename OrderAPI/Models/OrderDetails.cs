using System.ComponentModel.DataAnnotations.Schema;
using OrderAPI.Models.DTOs;

namespace OrderAPI.Models
{
	public class OrderDetails
	{
		public int Id { get; set; }
		public int OrderHeaderId { get; set; }
		[ForeignKey(nameof(OrderHeaderId))]
		public OrderHeader? OrderHeader { get; set; }
		public int ProductId { get; set; }
		[NotMapped]
		public ProductDTO? Product { get; set; }
		public int Count { get; set; }
		public string ProductName { get; set; }
		public double ProductPrice { get; set; }
	}
}
