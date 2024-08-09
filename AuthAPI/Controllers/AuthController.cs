using AuthAPI.Models.Dtos;
using AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponceDTO _responceDTO;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _responceDTO = new ResponceDTO();

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            var errorMessage = await _authService.Register(registrationRequestDTO);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responceDTO.Success = false;
                _responceDTO.Message= errorMessage;
                return BadRequest(_responceDTO);
            }
            _responceDTO.Success = true;
            return Ok(_responceDTO);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var loginresponce = await _authService.Login(loginRequestDTO);
            if(loginresponce.User == null)
            {   
                _responceDTO.Success= false;
                _responceDTO.Message = "Username or password is incorrect";
                return BadRequest(_responceDTO);
            }
            _responceDTO.Success = true;
            _responceDTO.Result = loginresponce;
            return Ok(_responceDTO);
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            var assignRoleSuccessful = await _authService.AssignRole(registrationRequestDTO.Email,registrationRequestDTO.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _responceDTO.Success = false;
                _responceDTO.Message = "Error occured";
                return BadRequest(_responceDTO);
            }
            _responceDTO.Success = true;
            return Ok(_responceDTO);
        }
    }
}
