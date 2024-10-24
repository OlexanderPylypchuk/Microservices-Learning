using AutoMapper;
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
		private readonly ApplicationDbContext _db;

        public OrderAPIController(IMapper mapper, ApplicationDbContext db)
        {
            _mapper = mapper;
			_db = db;
			_responce = new ResponceDTO();
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

				await _db.AddAsync(order);
				await _db.SaveChangesAsync();

				orderHeaderDTO.Id = order.Id;

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
