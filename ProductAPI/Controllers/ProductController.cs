using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;
using ProductAPI.Models.Dto;
using ProductAPI.Models.Dtos;

namespace ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ResponceDTO _responceDTO;
        public ProductController(ApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _responceDTO = new ResponceDTO();
        }
        [HttpGet]
        public async Task<ResponceDTO> Get()
        {
            try
            {
                var list = await _db.Products.ToListAsync();
                _responceDTO.Success = true;
                _responceDTO.Result = list;
            }
            catch (Exception ex)
            {
                _responceDTO.Success = false;
                _responceDTO.Message = ex.Message;
            }
            return _responceDTO;
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResponceDTO> Get(int id)
        {
            try
            {
                var item = await _db.Products.FirstOrDefaultAsync(u=>u.Id==id);
                _responceDTO.Success = true;
                _responceDTO.Result = _mapper.Map<ProductDTO>(item); 
            }
            catch (Exception ex)
            {
                _responceDTO.Success = false;
                _responceDTO.Message = ex.Message;
            }
            return _responceDTO;
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponceDTO> Create([FromBody] ProductDTO productDTO)
        {
            try
            {
                var product = _mapper.Map<Product>(productDTO);
                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();
                _responceDTO.Success = true;
                _responceDTO.Result = _mapper.Map<ProductDTO>(product);
            }
            catch(Exception ex)
            {
                _responceDTO.Success= false;
                _responceDTO.Message= ex.Message;
            }
            return _responceDTO;
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponceDTO> Update([FromBody]ProductDTO productDTO)
        {
            try
            {
                var product = _mapper.Map<Product>(productDTO);
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                _responceDTO.Success = true;
                _responceDTO.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _responceDTO.Success = false;
                _responceDTO.Message = ex.Message;
            }
            return _responceDTO;
        }
        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("{id:int}")]
        public async Task<ResponceDTO> Delete(int id)
        {
            try
            {
                var item = await _db.Products.FirstOrDefaultAsync(u=>u.Id == id);
                _db.Products.Remove(item);
                await _db.SaveChangesAsync();
                _responceDTO.Success = true;
            }
            catch(Exception ex)
            {
                _responceDTO.Success = false;
                _responceDTO.Message = ex.Message;
            }
            return _responceDTO;
        }
    }
}
