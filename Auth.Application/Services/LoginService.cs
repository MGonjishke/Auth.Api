using Auth.Domain.Common;
using Auth.Domain.Dtos;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly ILogger<LoginService> _logger;

        private readonly ITokenService _tokenService;


        public LoginService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<LoginService> logger, ITokenService tokenService )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<LoginResult> LoginAsync(LoginDto loginDto)
        {

            


            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Email == loginDto.UsernameOrEmail || user.UserName == loginDto.UsernameOrEmail);

            if (user == null)
            {
                _logger.LogWarning("User not find. Username or email: {UsernameOrEmail}", loginDto.UsernameOrEmail);

                return new LoginResult() { IsSuccess = false, Message = "Invalid username or email" };
            }

            var isPasswordMatch = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!isPasswordMatch.Succeeded)
            {
                _logger.LogWarning("Login failed. incorrect password for user {UsernameOrEmail}", user.UserName);

                return new LoginResult() { IsSuccess = false, Message = "Invalid Password" };
            }


            var token =  _tokenService.CreateToken(user);


            _logger.LogInformation("User logged in successfully. Username: {Username}", user.UserName);


            user.LastLoginDate = DateTime.UtcNow;


            await _userManager.UpdateAsync(user);

            return new LoginResult()
            {
                IsSuccess = true,
                Message = "Login successful",
                Token = token,
                UsernameOrEmail = loginDto.UsernameOrEmail
            };
        }
    }
}
