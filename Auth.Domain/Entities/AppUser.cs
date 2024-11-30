using Auth.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime? RegisterDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public AccountStatus Status { get; set; }

    }
}
