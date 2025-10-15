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
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

using Anade.Khadamat.Data_Authentification;
using Anade.Khadamat.Data_Inscription;
using Anade.Khadamat.Data_Suivi;
using Anade.Khadamat.Web.Services;

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

            services.AddScoped<DbContext, AuthentificationDbContext>();
            services.AddDbContext<AuthentificationDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("AuthentificationDbContext")));

            services.AddScoped<DbContext, InscriptionDbContext>();
            services.AddDbContext<InscriptionDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("InscriptionDbContext")));

            services.AddScoped<DbContext, SuiviDbContext>();
            services.AddDbContext<SuiviDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("SuiviDbContext")));

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

            // pour utiliser les compte dans l'inscription en ligne.
            services.AddTransient<CompteAuthentificationBusinesService>();

            // pour utiliser les compte dans l'inscription en ligne.
            services.AddTransient<PromoteurInscriptionBusinesService>();
            services.AddTransient<Detail_DossierInscriptionBusinesService>();
            services.AddTransient<AssociesInscriptionBusinesService>();
            services.AddTransient<Inscription_CategorieBusinesService>();
            services.AddTransient<Inscription_CirconscriptionBusinessService>();
            services.AddTransient<Inscription_CommuneBusinesService>();
            services.AddTransient<Inscription_AnnexeBusinesService>();
            services.AddTransient<Inscription_AntenneBusinesService>();

            // pour utiliser les compte dans l'inscription en ligne.
            services.AddTransient<PromoteurSuiviBusinesService>();

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

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            services.AddHttpContextAccessor();

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

            // Middleware pour supprimer l’en-tête X-Powered-By
            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Remove("X-Powered-By");
                    return Task.CompletedTask;
                });

                await next.Invoke();
            });

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
