using System;
using Anade.Khadamat.Identity;
using Anade.Khadamat.Identity.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Anade.Khadamat.Web.Areas.Identity.IdentityHostingStartup))]
namespace Anade.Khadamat.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((Action<WebHostBuilderContext, IServiceCollection>)((context, services) => {

                var dbconnection = context.Configuration.GetConnectionString("IdentityContextConnection");
                services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(dbconnection, x => x.MigrationsAssembly("Anade.Khadamat.Web")));

                services.AddDefaultIdentity<User>((Action<IdentityOptions>)(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.SignIn.RequireConfirmedAccount = true;

                }))
               .AddRoles<Role>()
               .AddEntityFrameworkStores<IdentityContext>();


                services.AddTransient<UserService>();
                services.AddTransient<UserStructureService>();
                services.AddTransient<TypeUserServices>();
                services.AddTransient<StructureService>();
                services.AddTransient<UserRoleService>();
                services.AddTransient<NiveauService>();



            }));


        }
    }

}