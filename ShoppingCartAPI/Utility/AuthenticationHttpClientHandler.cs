
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace ShoppingCartAPI.Utility
{
	public class AuthenticationHttpClientHandler : DelegatingHandler
	{
		private readonly IHttpContextAccessor _contextAccessor;

		public AuthenticationHttpClientHandler(IHttpContextAccessor contextAccessor)
		{
			_contextAccessor = contextAccessor;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			string token = await _contextAccessor.HttpContext.GetTokenAsync("access_token");

			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
