using AuthAPI.Models.Dtos;

namespace AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDTO registrationrequestDTO);
        Task<LoginResponceDTO> Login(LoginRequestDTO loginrequestDTO);
        Task<bool> AssignRole(string email, string role);
    }
}
