using Anade.Business.Core;
using Anade.Business.Core.recapcha;
using Anade.Data.Abstractions;
using Anade.Data.Core;
using Anade.Khadamat.Business;
using Anade.Khadamat.Data;
using Anade.Khadamat.Domain;
using Anade.Khadamat.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Anade.Khadamat.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddResponseCaching();
            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options =>
                                       options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.Configure<ConfigurationModel>(Configuration.GetSection("EmbedSettings"));
            services.AddScoped<DbContext, KhadamtDbContext>();
            services.AddDbContext<KhadamtDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("khadamatDbConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddTransient(typeof(IRepository<,>), typeof(GenericRepository<,>));
            services.AddTransient(typeof(ReferentielBusinessService<,>));
            services.AddTransient(typeof(GenericBusinessService<,>));
            services.AddTransient<InscriptionBusinessService>();
            services.AddTransient<CommuneBusinessService>();
            services.AddTransient<SessionFormationBusinessService>();
            services.AddTransient<CirconscriptionBusinessService>();


            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ILoadProperties<SessionFormationDetails, int>, LoadProperties<SessionFormationDetails, int>>();

            services.AddScoped<IPromoteurEligibiliteSettings, PromoteurEligibiliteSettings>();
            services.Configure<PromoteurEligibiliteSettings>(Configuration.GetSection("PromoteurEligibiliteSettings"));


            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailVerificationService, EmailVerificationService>();
            services.AddHttpClient();
            services.AddScoped<IRecaptchaService, RecaptchaService>();

            services.AddCors(o =>
            {
                o.AddDefaultPolicy(p =>
                {
                    p.AllowAnyOrigin();
                    p.AllowAnyMethod();
                    p.AllowAnyHeader();
                });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
