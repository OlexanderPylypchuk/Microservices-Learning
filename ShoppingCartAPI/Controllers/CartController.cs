using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Models.Dto;
using ShoppingCartAPI.Models.Dtos;

namespace ShoppingCartAPI.Controllers
{
	[Route("api/cart")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;
		protected ResponceDTO _responceDTO;
		public CartController(ApplicationDbContext db,IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
			_responceDTO = new ResponceDTO();
		}
		[HttpPost("cartupsert")]
		public async Task<ResponceDTO> CartUpsert(CartDTO cartDTO)
		{
			try
			{
				var headerfromdb = await _db.Headers.FirstOrDefaultAsync(h=>h.UserId==cartDTO.Header.UserId);
                if (headerfromdb == null)
				{
					var cartHeader = _mapper.Map<CartHeader>(cartDTO.Header);
					_db.Headers.Add(headerfromdb);
					await _db.SaveChangesAsync();
					cartDTO.Details.First().CartHeaderId = cartHeader.Id;
					_db.Details.Add(_mapper.Map<CartDetails>(cartDTO.Details.First()));
					await _db.SaveChangesAsync();
				}
				else
                {
					var detailsFromDb = await _db.Details.FirstOrDefaultAsync(d=>d.ProductId==cartDTO.Details.First().ProductId && d.CartHeaderId==headerfromdb.Id);
					if(detailsFromDb == null)
					{

					}
					else
					{

					}
				}
			}
			catch (Exception ex)
			{
				_responceDTO.Success = false;
				_responceDTO.Message = ex.Message;
			}
			return _responceDTO;
		}
	}
}
