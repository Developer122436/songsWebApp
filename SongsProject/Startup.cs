using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SongsProject.Models;
using System;

namespace SongsProject
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                  .SetBasePath(env.ContentRootPath)
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MyConnStr"),
                    opt => { opt.UseRowNumberForPaging(); }
            ));

            services.AddAuthentication()
                .AddGoogle(options =>
            {
                options.ClientId = "464206860093-3a2gp7tlnkfmqb3q5jct9cfevrhq7fil.apps.googleusercontent.com";
                options.ClientSecret = "30D9B8VviyssWHTvXMNSJL5a";
            })
            .AddFacebook(options =>
            {
                options.AppId = "414787619203517";
                options.AppSecret = "51995b65a494426373ec077ea9052175";
            });

            // Prevent attacks of web sites that uses only HTTP - website will be only in HTTPS
            // HSTS - response header that inform browser to connect to the website only using HTTPS
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddSingleton(Configuration);

            services.AddScoped<ISongRepository, EFSongRepository>();

            services.AddScoped(sp => SessionCart.GetCart(sp));

            services.AddHttpContextAccessor();

            services.AddTransient<IOrderRepository, EFOrderRepository>();

            services.AddSession();

            services.AddMemoryCache();

            services.AddDistributedMemoryCache();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role", "true").RequireRole("Admin"));

                options.AddPolicy("EditRolePolicy",
                    policy => policy.RequireClaim("Edit Role", "true").RequireRole("Admin"));

                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));

                options.AddPolicy("UserRolePolicy", policy =>
                  policy.RequireRole("User"));
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });


        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseDatabaseErrorPage();

                app.UseBrowserLink();

            }
            else
            {
                app.UseExceptionHandler("/Error");

                app.UseStatusCodePagesWithReExecute("/Error/{0}");

                app.UseHsts();
            }

            app.UseDefaultFiles();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSession();

            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "Default",
                    template: "{controller=Home}/{action=ListCountry}/{Country?}"
                );

            });


        }

    }
}