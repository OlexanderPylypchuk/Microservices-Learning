using EmailAPI.Models.Dto;
using EmailAPI.Models.DTOs;

namespace EmailAPI.Services.IServices
{
	public interface IEmailSender
	{
		public Task EmailCartAndLog(CartDTO cart);
		public Task EmailRegisterAndLog(string email);
		public Task LogOrderPlaced(RewardsDTO rewardsDTO);
	}
}
