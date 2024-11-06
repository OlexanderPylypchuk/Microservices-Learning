using RewardAPI.Models.DTOs;

namespace RewardAPI.Services.IServices
{
	public interface IRewardService
	{
		Task<bool> UpdateRewards(RewardsDTO rewardsDTO);
	}
}
