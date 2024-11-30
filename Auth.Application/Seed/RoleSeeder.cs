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



            var roles = new[] { "Admin", "User", "Guest" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }



            var adminEmail = "admin@gmail.com";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new AppUser
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    RegisterDate = DateTime.Now,
                };


               var result = await userManager.CreateAsync(admin, "AdminPassword258");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin"); 
                }

            }




        }
    }
}
