using Auth.Domain.Common;
using Auth.Domain.Dtos;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<IdentityResult> DeleteUserAsync(string id);
    }
}
