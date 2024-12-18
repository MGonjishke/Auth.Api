using Auth.Application.Mappers;
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
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,ILogger<UserService> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            if (users.Count == 0)
            {
                _logger.LogError("there is no user");
                throw new Exception("There is no user");
            }
            
            var userDto =  users.Select(user => user.ToUserDto());

            return  userDto;
        }


        public async Task<IdentityResult> DeleteUserAsync(string id) 
        {


            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                _logger.LogWarning("There is no user with this ID");

                return  IdentityResult.Failed(new IdentityError { Description = "There is no user with this ID" });
            }

           var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return IdentityResult.Success;
            }

            var errors = result.Errors.Select(error => new IdentityError
            {
                Code = error.Code,
                Description = error.Description,
            }).ToArray();

            foreach (var error in errors)
            {
                _logger.LogError("Error: {error}",error);
            }

            return IdentityResult.Failed(errors);
        }
    }
}
