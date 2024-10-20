using System.Net;
using System.Net.Mail;
using System.Text;
using EmailAPI.Data;
using EmailAPI.Models;
using EmailAPI.Models.Dto;
using EmailAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace EmailAPI.Services
{
	public class EmailSender : IEmailSender
	{
		private DbContextOptions<ApplicationDbContext> _dbContextOptions;
		private string SenderEmail;
		private string Password;

        public EmailSender(DbContextOptions<ApplicationDbContext> dbContextOptions, IConfiguration configuration)
        {
			_dbContextOptions = dbContextOptions;
			SenderEmail = configuration.GetValue<string>("Credentials:Email");
			Password = configuration.GetValue<string>("Credentials:PW");
		}

        public async Task EmailCartAndLog(CartDTO cart)
		{
			StringBuilder message = new StringBuilder();
			message.Append("<h1>Your Cart<h1>");
			message.AppendLine("<br/>Total: "+cart.Header.CartTotal);
			message.Append("<br/>");
			message.Append("<ul>");
			foreach(var item in cart.Details)
			{
				message.Append("<li>");
				message.Append(item.ProductDto.Price + " x " + item.Count);
				message.Append("</li>");
			}
			message.Append("</ul>");

			await LogAndEmail(message.ToString(), cart.Header.Email);
		}

		public async Task<bool> LogAndEmail(string message, string email)
		{
			try
			{
				EmailLogger emailLogger = new EmailLogger()
				{
					Email = email,
					Message = message,
					EmailSent = DateTime.Now
				};
				var client = new SmtpClient()
				{
					EnableSsl = true,
					Credentials = new NetworkCredential(SenderEmail, Password)
				};
				await client.SendMailAsync(new MailMessage(
					from: SenderEmail,
					to: email,
					subject: "Your cart",
					body: message.ToString()
					));
				await using var _db = new ApplicationDbContext(_dbContextOptions);
				_db.EmailLogs.Add(emailLogger);
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
