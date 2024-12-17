using Auth.Domain.Dtos;
using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this AppUser user)
        {
            return new UserDto
            {
                FullName = user.FullName,
                Username = user.UserName,
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                LastLoginDate = user.LastLoginDate,
            };
        }
    }
}
