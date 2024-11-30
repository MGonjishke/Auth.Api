using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Common
{
    public class LoginResult
    {
        public string Message { get; set; }
        public string Token { get; set; }

        public bool IsSuccess { get; set; }

        public string UsernameOrEmail { get; set; }
    }
}
