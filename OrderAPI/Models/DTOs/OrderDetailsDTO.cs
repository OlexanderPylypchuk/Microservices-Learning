using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Models.DTOs
{
	public class OrderDetailsDTO
	{
		public int Id { get; set; }
		public int OrderHeaderId { get; set; }
		public OrderHeader? OrderHeader { get; set; }
		public int ProductId { get; set; }
		public ProductDTO? Product { get; set; }
		public int Count { get; set; }
		public string ProductName { get; set; }
		public double ProductPrice { get; set; }
	}
}
