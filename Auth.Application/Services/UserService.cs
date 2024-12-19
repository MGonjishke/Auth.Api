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
                return new List<UserDto>();
            }

            List<UserDto> userDtos = new List<UserDto>();

            foreach(var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                var userRole = role.FirstOrDefault();
                userDtos.Add(user.ToUserDto(userRole));
            }


            return userDtos;
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

        public async Task<IEnumerable<UserDto>> SearchUserAsync(string query)
        {

            var users = await _userManager.Users.Where(user =>
                user.Email.ToLower().Contains(query.ToLower()) || user.UserName.ToLower().Contains(query.ToLower()) || user.PhoneNumber.ToLower().Contains(query.ToLower())).ToListAsync();

            if (users.Count == 0)
            {
                _logger.LogWarning("There is no user.");
                return new List<UserDto>();
            }


            List<UserDto> userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                var userRole = role.FirstOrDefault();
                userDtos.Add(user.ToUserDto(userRole));
            }


            return userDtos;

        }

        public async Task<IdentityResult> ChangeRoleToAdmin(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                _logger.LogWarning("There is no user with this Id");
                return IdentityResult.Failed(new IdentityError { Description = "There is no user with this Id" });
            }

            if (_userManager.GetRolesAsync(user).ToString() == "Admin")
            {
                return IdentityResult.Failed(new IdentityError { Description = "The role is already admin" });
            }

            await _userManager.RemoveFromRoleAsync(user, "User");

            var result = await _userManager.AddToRoleAsync(user, "Admin");


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
                _logger.LogError("Error:{error}", error);
            }

            return IdentityResult.Failed(errors);

        }


    }
}
