using Microsoft.EntityFrameworkCore;
using RewardAPI.Services.IServices;
using RewardAPI.Data;
using RewardAPI.Models.DTOs;
using RewardAPI.Models;

namespace RewardAPI.Services
{
	public class RewardService:IRewardService
	{
		private DbContextOptions<ApplicationDbContext> _dbContextOptions;

		public RewardService(DbContextOptions<ApplicationDbContext> dbContextOptions, IConfiguration configuration)
		{
			_dbContextOptions = dbContextOptions;
		}

		public async Task<bool> UpdateRewards(RewardsDTO rewardsDTO)
		{
			try
			{
				Rewards rewards = new Rewards()
				{
					OrderId = rewardsDTO.OrderId,
					RewardTime = DateTime.Now,
					UserId = rewardsDTO.UserId
				};
				await using var _db = new ApplicationDbContext(_dbContextOptions);
				await _db.Rewards.AddAsync(rewards);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
