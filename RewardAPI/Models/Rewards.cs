namespace RewardAPI.Models
{
	public class Rewards
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public DateTime RewardTime { get; set; }
		public int OrderId { get; set; }
	}
}
