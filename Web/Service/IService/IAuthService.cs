using Micro.Web.Models;

namespace Micro.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponceDTO?> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<ResponceDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
        Task<ResponceDTO?> AssignRoleAsync(RegistrationRequestDTO assignRoleRequestDTO);
    }
}
