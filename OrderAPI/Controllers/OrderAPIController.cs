using AutoMapper;
using Micro.MessageBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Data;
using OrderAPI.Models;
using OrderAPI.Models.DTOs;
using OrderAPI.Utility;

namespace OrderAPI.Controllers
{
	[Route("api/order")]
	[ApiController]
	public class OrderAPIController : ControllerBase
	{
		protected ResponceDTO _responce;
		private IMapper _mapper;
		private readonly IMessageBus _messageBus;
		private readonly IConfiguration _configuration;
		private readonly ApplicationDbContext _db;

        public OrderAPIController(IMapper mapper, ApplicationDbContext db, IMessageBus messageBus, IConfiguration configuration)
        {
            _mapper = mapper;
			_db = db;
			_responce = new ResponceDTO();
			_messageBus = messageBus;
			_configuration = configuration;
        }

		[Authorize]
		[HttpPost("CreateOrder")]
		public async Task<ResponceDTO> CreateOrder([FromBody]CartDTO cartDTO)
		{
			try
			{
				OrderHeaderDTO orderHeaderDTO = _mapper.Map<OrderHeaderDTO>(cartDTO.Header);
				orderHeaderDTO.OrderTime = DateTime.Now;
				orderHeaderDTO.Status = SD.Status_Pending;
				orderHeaderDTO.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDTO>>(cartDTO.Details);

				OrderHeader order = _mapper.Map<OrderHeader>(orderHeaderDTO);
				order.Id=0;
				foreach(var orderDetail in order.OrderDetails)
				{
					orderDetail.Id= 0;
				}

				await _db.AddAsync(order);	
				await _db.SaveChangesAsync();
				orderHeaderDTO.Id = order.Id;
				RewardsDTO rewardsDTO = new RewardsDTO()
				{
					OrderId = order.Id,
					RewardActivity = Convert.ToInt32(order.OrderTotal),
					UserId = orderHeaderDTO.UserId
				};

				string topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
				await _messageBus.PublishMessage(rewardsDTO, topic);

				_responce.Result = orderHeaderDTO;
				_responce.Success = true;
			}
			catch (Exception ex)
			{
				_responce.Success = false;
				_responce.Message = ex.Message;
			}
			return _responce;
		}
    }
}
