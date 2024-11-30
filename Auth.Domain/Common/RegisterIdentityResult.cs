using Auth.Domain.Dtos;
using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Common
{
    public class RegisterIdentityResult<T>
    {
        public T? RegisterResult { get; set; }

        public NewUserDto NewUserDtoResult {  get; set; }

    }
}
