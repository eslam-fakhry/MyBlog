using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Areas.Identity.Data;

namespace MyBlog.Models
{
    public static class SeedAdminCredentials
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var userManager = serviceProvider.GetRequiredService<UserManager<MyBlogUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var x = await roleManager.RoleExistsAsync("Admin");
            if (!x)
            {
                var role = new IdentityRole {Name = "Admin"};
                await roleManager.CreateAsync(role);

                var user = new MyBlogUser {UserName = "admin", Email = config["AdminEmail"]};
                var userPwd = config["AdminPassword"];
                var createdUser = await userManager.CreateAsync(user, userPwd);

                if (createdUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
