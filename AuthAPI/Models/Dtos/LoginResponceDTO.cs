namespace AuthAPI.Models.Dtos
{
    public class LoginResponceDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}
