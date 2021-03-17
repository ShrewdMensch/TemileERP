using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;
using AutoMapper;
using Utility;
using Microsoft.AspNetCore.HttpOverrides;
using Utility.Notifications;

namespace Web
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (WebHostEnvironment.EnvironmentName == "Development")
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseLazyLoadingProxies();
                    options.UseSqlite(
                        Configuration.GetConnectionString("DevelopmentDbConnection"), x => x.MigrationsAssembly("SQLiteMigrations"));
                });
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseLazyLoadingProxies();
                    options.UseMySql(
                        Configuration.GetConnectionString("ProductionDbConnection"), x => x.MigrationsAssembly("MySqlMigrations"));
                });

            }

            StartUpConfigureServices(services);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void ConfigureDevelopment(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            StartUpConfigure(app);
        }

        public void ConfigureProduction(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

            StartUpConfigure(app);
        }

        private void StartUpConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddDefaultTokenProviders();

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/");
                options.Conventions.AddPageRoute("/Account/Login", "");
            }).AddRazorRuntimeCompilation();




            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 1;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "ApplicationCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Account/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.AddAutoMapper(typeof(IRepository));
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<ISend, Send>();
            services.AddSingleton(Configuration.GetSection("SendGridSetting").Get<SendGridSetting>());
        }

        private void StartUpConfigure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            AddSecurityHeaders(app);
            AddContentSecurityPolicy(app);

            app.UseStaticFiles();


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            app.UseStatusCodePagesWithRedirects("/Error?code={0}");
        }

        private static void AddSecurityHeaders(IApplicationBuilder app)
        {
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
            app.UseXfo(opt => opt.Deny());
        }
        private static void AddContentSecurityPolicy(IApplicationBuilder app)
        {
            app.UseCsp(opt => opt
                .BlockAllMixedContent()

                .StyleSources(s => s.Self().CustomSources("https://fonts.googleapis.com",
                "sha256-Y5HGV3cmFL1QmdV9FMkQjm7MR7FR+stNxbf9+GKET60=",
                "sha256-47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU=",
                "sha256-t6oewASd7J1vBg5mQtX4hl8bg8FeegYFM3scKLIhYUc="))

                .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com"))
                .FormActions(s => s.Self())
                .FrameAncestors(s => s.Self())
                .ImageSources(s => s.Self())

                .ScriptSources(s => s.Self().UnsafeEval()
                .CustomSources("sha256-ehPVrgdV2GwJCE7DAMSg8aCgaSH3TZmA66nZZv8XrTg=",
                "sha256-bXzhOTGGMCZbG9CGP2hWDIT3FLu3HWHtJVwHo3YKTys=",
                "sha256-bYwdB16yXTuYngDpcLvxximXKlVk0H49c/Wfxn7xxnY=",
                "sha256-cb/G0bW4bFg70i/kY23hth4jNb3wz/pepJTipMtzupw="))
            );
        }
    }
}
