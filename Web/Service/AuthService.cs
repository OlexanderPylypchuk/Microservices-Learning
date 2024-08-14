using Micro.Web.Models;
using Micro.Web.Service.IService;

namespace Micro.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponceDTO?> AssignRoleAsync(RegistrationRequestDTO assignRoleRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                Type = SD.ApiType.POST,
                Url = SD.AuthApiBase + "/api/auth/AssignRole",
                Data = assignRoleRequestDTO
            });
        }

        public async Task<ResponceDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Type = SD.ApiType.POST,
                Url = SD.AuthApiBase + "/api/auth/login",
                Data = loginRequestDTO
            }, withBearer: true);
        }

        public async Task<ResponceDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                Type = SD.ApiType.POST,
                Url = SD.AuthApiBase + "/api/auth/register",
                Data = registrationRequestDTO
            }, withBearer:true);
        }
    }
}
