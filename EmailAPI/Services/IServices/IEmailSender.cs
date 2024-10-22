using EmailAPI.Models.Dto;

namespace EmailAPI.Services.IServices
{
	public interface IEmailSender
	{
		public Task EmailCartAndLog(CartDTO cart);
		public Task EmailRegisterAndLog(string email);
	}
}
