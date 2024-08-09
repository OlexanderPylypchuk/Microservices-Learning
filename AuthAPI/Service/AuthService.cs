﻿using AuthAPI.Data;
using AuthAPI.Models;
using AuthAPI.Models.Dtos;
using AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager; 
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<LoginResponceDTO> Login(LoginRequestDTO loginrequestDTO)
        {
            var user = _db.Users.FirstOrDefault(u=>u.UserName.ToLower()==loginrequestDTO.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginrequestDTO.Password);

            if (!isValid||user==null)
            {
                return new LoginResponceDTO()
                {
                    User=null,
                    Token=""
                };
            }
            var token = _jwtTokenGenerator.GenerateToken(user);
            UserDTO userDTO = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber

            };

            LoginResponceDTO loginResponceDTO = new LoginResponceDTO()
            {
                User = userDTO,
                Token = token
            };
            return loginResponceDTO;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationrequestDTO)
        {
            ApplicationUser applicationUser = new()
            {
                UserName = registrationrequestDTO.Email,
                Email = registrationrequestDTO.Email,
                NormalizedEmail = registrationrequestDTO.Email.ToUpper(),
                PhoneNumber = registrationrequestDTO.PhoneNumber,
                Name = registrationrequestDTO.Name,
            };
            try
            {
                var result = await _userManager.CreateAsync(applicationUser,registrationrequestDTO.Password);
                if (result.Succeeded)
                {
                    var user = _db.Users.Where(u=>u.Email==registrationrequestDTO.Email).FirstOrDefault();
                    UserDTO userDTO = new()
                    {
                        Email = user.Email,
                        Id = user.Id,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber

                    };
                    return "";
                }
                return result.Errors.FirstOrDefault().Description;
            }
            catch (Exception ex)
            {

            }
            return "Error occured";
        }

        public async Task<bool> AssignRole(string email, string role)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
                await _userManager.AddToRoleAsync(user,role);
                return true;
            }
            return false;
        }
    }
}