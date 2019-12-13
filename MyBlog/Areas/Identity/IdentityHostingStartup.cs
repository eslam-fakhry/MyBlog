using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Areas.Identity.Data;
using MyBlog.Data;

[assembly: HostingStartup(typeof(MyBlog.Areas.Identity.IdentityHostingStartup))]

namespace MyBlog.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<MyBlogContext>(options =>
                {
                    options.UseNpgsql(context.Configuration.GetConnectionString("MyBlogContextConnection"));
                });

                services.AddDefaultIdentity<MyBlogUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<MyBlogContext>();
            });
        }
    }
}
