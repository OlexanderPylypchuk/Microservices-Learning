using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Micro.Web.Models;
using Micro.Web.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Micro.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO();
            return View(loginRequestDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            if(ModelState.IsValid)
            {
                var responce = await _authService.LoginAsync(loginRequestDTO);
                if (responce.Success)
                {
                    LoginResponceDTO responceDTO = JsonConvert.DeserializeObject<LoginResponceDTO>(Convert.ToString(responce.Result));
                    await SignInUser(responceDTO);
                    _tokenProvider.SetToken(responceDTO.Token);
                    TempData["success"] = "Loged in succesfuly";
                }
                else
                {
                    TempData["error"] = responce.Message;
                }
            }
            return RedirectToAction("Index", "Home");
        }
        private async Task SignInUser(LoginResponceDTO model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u=>u.Type== JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
			identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

			identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
			identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

			var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
			return RedirectToAction("Index", "Home");
		}
        [HttpGet]
        public IActionResult Register()
        {
            RegistrationRequestDTO registrationRequestDTO = new RegistrationRequestDTO();
            ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem(SD.RoleAdmin,SD.RoleAdmin),
                new SelectListItem(SD.RoleCustomer,SD.RoleCustomer)
            };
            return View(registrationRequestDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterAsync(registrationRequestDTO);
                if(result.Success)
                {
                    if (string.IsNullOrEmpty(registrationRequestDTO.Role))
                    {
                        registrationRequestDTO.Role = SD.RoleCustomer;
                    }
                    var assignRole = await _authService.AssignRoleAsync(registrationRequestDTO);
                    if(assignRole != null&& assignRole.Success)
					    TempData["success"] = "Registration completed succesfuly";
				}
                else TempData["error"] = result.Message;
			}
            return RedirectToAction("Index", "Home");
        }
    }
}
