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
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterService> _logger;
        private readonly ITokenService _tokenService;
        public RegisterService(UserManager<AppUser> userManager, ILogger<RegisterService> logger, ITokenService tokenService) 
        {
            _userManager = userManager;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<RegisterIdentityResult<IdentityResult>> RegisterAsync(RegisterDto registerDto)
        {



            if (!ValidationHelper.IsValidEmail(registerDto.Email))
            {
                _logger.LogWarning("Invalid email format: {email}", registerDto.Email);

                return new RegisterIdentityResult<IdentityResult>()
                {
                    RegisterResult = IdentityResult.Failed(new IdentityError { Description = "Invalid email format" })
                };
            }

            _logger.LogInformation("Checkign if username or email already exist");

            var existUser = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == registerDto.Username || user.Email == registerDto.Email || user.PhoneNumber == registerDto.PhoneNumber);

            if (existUser != null)
            {
                _logger.LogWarning("Emali,Username,Phonenumber already exist. Email: {Email}, Username: {Username}, Number: {Number}", registerDto.Email, registerDto.Username, registerDto.PhoneNumber);

                var error = existUser.UserName == registerDto.Username ? "Username already exist" : "Email already exist";

                return new RegisterIdentityResult<IdentityResult>()
                {
                    RegisterResult = IdentityResult.Failed(new IdentityError { Description = error })
                };
            }


            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.Fullname,
                PhoneNumber = registerDto.PhoneNumber,
                Status = Domain.Enums.AccountStatus.Active,
                RegisterDate = DateTime.UtcNow,
            };

            _logger.LogInformation("Attemping to create user. Email: {Email}, Username: {Username}", registerDto.Email, registerDto.Username);

            var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);


            if (createUser.Succeeded)
            {
                _logger.LogInformation("User created. Email: {Email}, Username: {Username}", registerDto.Email, registerDto.Username);

                await _userManager.AddToRoleAsync(appUser, "User");

                return new RegisterIdentityResult<IdentityResult>()
                {
                    RegisterResult = IdentityResult.Success,
                    NewUserDtoResult = new NewUserDto
                    {
                        Email = registerDto.Email,
                        UserName = registerDto.Username,
                        Token = await  _tokenService.CreateToken(appUser)
                    }
                };
            }
            else
            {
                var errors = createUser.Errors.Select(error => new IdentityError
                {
                    Code = error.Code,
                    Description = error.Description,
                }).ToArray();

                foreach (var error in errors)
                {
                    _logger.LogError("Error. Reason: {Error}", error);
                }

                return new RegisterIdentityResult<IdentityResult>()
                {
                    RegisterResult = IdentityResult.Failed(errors)
                };

            }
        }


    }
}
