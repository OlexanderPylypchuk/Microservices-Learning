using AutoMapper;
using CouponAPI.Models;
using CouponAPI.Models.Dto;
using CouponAPIAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponAPI.Controllers
{
	[Route("api/coupon")]
	[ApiController]
    [Authorize]
    public class CouponController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private ResponceDTO _responce;
        public CouponController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
			_mapper = mapper;
			_responce = new ResponceDTO();
        }

		[HttpGet]
		public ResponceDTO Get()
		{
			try
			{
				IEnumerable<Coupon> objList = _context.Coupons.ToList();
				_responce.Result = _mapper.Map<IEnumerable<CouponDTO>>(objList);
				_responce.Success = true;
				return _responce;
			}
			catch (Exception ex)
			{
				_responce.Success=false;
				_responce.Message = ex.Message;
				return _responce;
			}
		}
		[HttpGet]
		[Route("{id:int}")]
		public ResponceDTO Get(int id)
		{
			try
			{
				Coupon obj = _context.Coupons.First(u => u.Id == id);
				_responce.Result =	_mapper.Map<CouponDTO>(obj);
				_responce.Success = true;
				return _responce;
			}
			catch (Exception ex)
			{
				_responce.Success = false;
				_responce.Message = ex.Message;
				return _responce;
			}
		}
		[HttpGet]
		[Route("GetByCode/{code}")]
		public ResponceDTO GetByCode(string code)
		{
			try
			{
				Coupon obj = _context.Coupons.FirstOrDefault(u => u.CouponCode.ToLower() == code.ToLower());
				_responce.Result = _mapper.Map<CouponDTO>(obj);
				_responce.Success = true;
				return _responce;
			}
			catch (Exception ex)
			{
				_responce.Success = false;
				_responce.Message = ex.Message;
				return _responce;
			}
		}
		[HttpPost]
		[Authorize(Roles = "ADMIN")]
		public ResponceDTO Post([FromBody]CouponDTO couponDTO)
		{
			try
			{
				Coupon coupon = _mapper.Map<Coupon>(couponDTO);
				_context.Coupons.Add(coupon);
				_context.SaveChanges();
				_responce.Result = _mapper.Map<CouponDTO>(coupon);
				_responce.Success = true;
				return _responce;
			}
			catch(Exception ex)
			{
				_responce.Success = false;
				_responce.Message = ex.Message;
			}
			return _responce;
		}
		[HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponceDTO Put([FromBody] CouponDTO couponDTO)
		{
			try
			{
				Coupon coupon = _mapper.Map<Coupon>(couponDTO);
				_context.Coupons.Update(coupon);
				_context.SaveChanges();
				_responce.Result = _mapper.Map<CouponDTO>(coupon);
				_responce.Success = true;
				return _responce;
			}
			catch (Exception ex)
			{
				_responce.Success = false;
				_responce.Message = ex.Message;
			}
			return _responce;
		}
		[HttpDelete]
		[Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponceDTO Delete(int id)
		{
			try
			{
				Coupon coupon = _context.Coupons.First(u=>u.Id==id);
				_context.Coupons.Remove(coupon);
				_context.SaveChanges();
				_responce.Success = true;
				return _responce;
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
