using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Auth.Domain.Common
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            if  (string.IsNullOrEmpty(email))
                return false;

            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
