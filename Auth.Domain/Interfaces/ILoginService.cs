using Auth.Domain.Common;
using Auth.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResult> LoginAsync(LoginDto loginDto);
    }
}
