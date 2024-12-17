using Auth.Application.Mappers;
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
    }
}
