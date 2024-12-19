using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Seed
{
    public static class RoleSeeder
    {


        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();



            var roles = new[] {"SuperAdmin", "Admin", "User"};

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }



            var adminEmail = "SuperAdmin@gmail.com";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new AppUser
                {
                    UserName = "SuperAdmin",
                    Email = adminEmail,
                    RegisterDate = DateTime.Now,
                };


               var result = await userManager.CreateAsync(admin, "Password123?");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "SuperAdmin"); 
                }

            }




        }
    }
}
