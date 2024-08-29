﻿using System.Reflection.PortableExecutable;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Models.Dto;
using ShoppingCartAPI.Models.Dtos;
using ShoppingCartAPI.Service.IService;

namespace ShoppingCartAPI.Controllers
{
	[Route("api/cart")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;
		private readonly IProductService _productService;
		protected ResponceDTO _responceDTO;
		public CartController(ApplicationDbContext db,IMapper mapper, IProductService productService)
		{
			_db = db;
			_mapper = mapper;
			_responceDTO = new ResponceDTO();
			_productService = productService;
		}
		[HttpGet("getcart/{userid}")]
		public async Task<ResponceDTO> GetCart(string userid)
		{
			try
			{
				CartDTO cart = new()
				{
					Header = _mapper.Map<CartHeaderDTO>(_db.Headers.First(u => u.UserId == userid))
				};
				cart.Details = _mapper.Map<IEnumerable<CartDetailsDTO>>(_db.Details.Where(u=>u.CartHeaderId==cart.Header.Id).ToList());

				IEnumerable<ProductDTO> productDTOs = await _productService.GetAllProductsAsync();
				foreach(var item in cart.Details)
				{
					item.ProductDto = productDTOs.FirstOrDefault(u => u.Id == item.ProductId);
					cart.Header.CartTotal += item.Count * item.ProductDto.Price;
				}
				_responceDTO.Success = true;
				_responceDTO.Result = cart;
			}
			catch (Exception ex)
			{
				_responceDTO.Success = false;
				_responceDTO.Message = ex.Message;
			}
			return _responceDTO;
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
						cartDTO.Details.First().CartHeaderId = headerfromdb.Id;
						cartDTO.Details.First().CartHeader = headerfromdb;
						_db.Details.Add(_mapper.Map<CartDetails>(cartDTO.Details.First()));
						await _db.SaveChangesAsync();
					}
					else
					{
						detailsFromDb.CartHeaderId = headerfromdb.Id;
						detailsFromDb.Count += cartDTO.Details.First().Count;
						_db.Update(detailsFromDb);
						await _db.SaveChangesAsync();
					}
				}
				_responceDTO.Success = true;
				_responceDTO.Result = cartDTO;
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
