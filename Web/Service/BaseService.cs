﻿using System.Text;
using System.Text.Json.Serialization;
using Micro.Web.Models;
using Micro.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Linq.Expressions;

namespace Micro.Web.Service
{
	public class BaseService : IBaseService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		public BaseService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		public async Task<ResponceDTO?> SendAsync(RequestDTO requestDTO)
		{
			try
			{
				HttpClient client = _httpClientFactory.CreateClient("CouponAPI");
				HttpRequestMessage message = new();
				message.Headers.Add("Accept", "application/json");
				//Потім додам токени

				message.RequestUri = new Uri(requestDTO.Url);
				if (requestDTO.Data != null)
				{
					message.Content = new StringContent(JsonConvert.SerializeObject(requestDTO.Data), Encoding.UTF8, "application/json");
				}

				switch (requestDTO.Type)
				{
					case SD.ApiType.POST:
						message.Method = HttpMethod.Post; break;
					case SD.ApiType.PUT:
						message.Method = HttpMethod.Put; break;
					case SD.ApiType.DELETE:
						message.Method = HttpMethod.Delete; break;
					default:
						message.Method = HttpMethod.Get; break;
				}

				HttpResponseMessage? responseMessage = await client.SendAsync(message);

				switch (responseMessage.StatusCode)
				{
					case HttpStatusCode.NotFound:
						return new ResponceDTO()
						{
							Success = false,
							Message = "Not found"
						};
					case HttpStatusCode.Forbidden:
						return new ResponceDTO()
						{
							Success = false,
							Message = "Not found"
						};
					case HttpStatusCode.InternalServerError:
						return new ResponceDTO()
						{
							Success = false,
							Message = "Not found"
						};
					case HttpStatusCode.Unauthorized:
						return new ResponceDTO()
						{
							Success = false,
							Message = "Not found"
						};
					default:
						var content = await responseMessage.Content.ReadAsStringAsync();
						var apiResponce = JsonConvert.DeserializeObject<ResponceDTO>(content);
						return apiResponce;
				}
				
			}
			catch (Exception ex)
			{
				return new ResponceDTO()
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
	}
}