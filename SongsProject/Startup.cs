using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SongsProject.Models;
using Microsoft.AspNetCore.Identity;

namespace SongsProject
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        // Access Configuration Sources is Using IConfiguration Service
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            // Configuration Sources have appsettings.json,appsettings.Development.jason,
            // User secrets,Environment variables and Command-line arguments
            // Command-line override environment variable
            Configuration = configuration;

                var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json",
                         optional: false,
                         reloadOnChange: true)
            .AddEnvironmentVariables();

                if (env.IsDevelopment())
                {
                    builder.AddUserSecrets<Startup>();
                }

                Configuration = builder.Build();
        }

        // This method gets called by the runtime. 
        // Use this method to add services to the dependency injection container.
        public void ConfigureServices(IServiceCollection services)
        {           
            // Add the filter globally MVC.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //This allows us to communicate with the database wherever 
            //we need to within the app without manually having to create new instances.
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

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("songsproject.com");
                options.ExcludedHosts.Add("www.songsproject.com");
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5001;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                // options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            
            //AddIdentity() method adds the default identity system configuration for the specified user and role types. IdentityUser class is provided by ASP.NET core
            //and contains properties for UserName, PasswordHash, Email etc. This is the class that is used 
            //by default by the ASP.NET Core Identity framework to manage registered users of your application.
            //Similarly, IdentityRole is also a builtin class provided by ASP.NET Core Identity and contains Role information.
            //role can be adminstrator, normal person and more.
            // Override the default rules for passwords from passwordOptions class.
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
            //By adding it as a singleton, whenever we access it from another class, 
            //it will always be the same instance throughout the app (Single instance).
            //It is first used from classes that we created.
            //That instance is shared between all components that require it. 
            //The same instance is thus used always.
            //Singleton have the biggest lifetime more than scope and transient.
            //A component cannot depend on components with a lifetime smaller than their own.
            services.AddSingleton(Configuration);
            /*MVC emphasizes the use of loosely coupled components, which means you can make a change in one part
            of the application without having to make corresponding changes elsewhere.
            This approach categorizes
            parts of the application as services, which provide features that other parts of the application use.
            Here is defined a new service for the repository
            The statement I added to the ConfigureServices method tells ASP.NET Core that when a component,
            such as a controller, needs an implementation of the ISongRepository interface.
            The AddTransient method specifies that a new
            fake repository object should be created each time the ISongRepository interface is needed.
            Don’t worry if this doesn’t make sense at the moment; you will see how it fits into the application shortly,
            fake data will be seamlessly replaced by the real data in the database without having
            to change the SongsController class.*/
            services.AddScoped<ISongRepository, EFSongRepository>();
            //The AddScoped method specifies that the same object should be used to satisfy 
            //related requests for Cart instances.
            /*Rather than provide the AddScoped method with a type mapping, as I did for the repository, I have
            specified a lambda expression that will be invoked to satisfy Cart requests. The expression receives the
            collection of services that have been registered and passes the collection to the GetCart method of the
            SessionCart class. The result is that requests for the Cart service will be handled by creating SessionCart
            objects, which will serialize themselves as session data when they are modified.*/
            //Scoped means an instance is created once per scope. A scope is created on every request to the application,
            //thus any components registered as Scoped will be created once per request.
            services.AddScoped(sp => SessionCart.GetCart(sp));
            /*I also added a service using the AddSingleton method, which specifies that the same object
            should always be used. The service I created tells MVC to use the HttpContextAccessor class when
            implementations of the IHttpContextAccessor interface are required.*/
            services.AddHttpContextAccessor();
            //Transient components are created every time they are requested and are never shared.
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            // registers the services used to access session data.
            // session state which is data that is stored at the server
            // and associated with a series of requests made by a user.
            services.AddSession();
            //The AddMemoryCache method call sets up the in-memory data store.
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            services.AddAuthorization(options =>
            {
                // ClaimType comparison is case in-sensitive, ClaimValue comparison is case sensitive
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

        //This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //The IHostingEnvironment is used typically to setup configuration for the application.
        //So you can request components at configuration time through ApplicationServices 
        //on the IApplicationBuilder(app is create middlewares such as app.UseDeveloperExceptionPage() for the requests coming from https)
        //and at request time through RequestServices on the HttpContext.
        //The default hosting environment is production - had better security and performance.
        //We need update another default on environment variables on control panel.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // On LaunchSettings.json we had details about development hosting
            // On properties we choose environment variable
            if (env.IsDevelopment())
            {
                //Captures synchronous and asynchronous Exception instances from the pipeline
                //and generates HTML error responses 
                //Using this page on a non-development environment like Production for example is a security risk as it contains detailed exception information that could be used by an attacker.
                //Also this exception page does not make any sense to the end user.
                //if there is mentioned "throw new some exception"
                app.UseDeveloperExceptionPage();
                //Captures synchronous and asynchronous database related exceptions from the pipeline that may be resolved using Entity Framework migrations. 
                //When these exceptions occur an HTML response with details of possible actions to resolve the issue is generated.
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();

            }
            else
            {
                //Adds a middleware to the pipeline that will catch exceptions, log them, and re-execute the request in an alternate pipeline. 
                //It is handle Error 500: means, there was an error on the server which the server did not know how to handle.
                app.UseExceptionHandler("/Error");
                // Adds a middleware to the pipeline that will catch exceptions, log them, and re - execute the request in an alternate pipeline.
                //UseStatusCodePagesWithWithRedirects- If the request finds status 302 (not exist) on URL request of user,it moves "Error/404" to app.UseMVC()
                //And updates not original status 200(means the request completed successfully) to Error/404 and show it on search.
                //UseStatusCodePagesWithReExecute re execute the pipeline 
                //And will show original URL request on search bar and not changed to Error/404.
                //And the status will be original status code 404.
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                //Adds middleware for using HSTS, which adds the Strict-Transport-Security header.
                app.UseHsts();
            }
            //This middleware adds a simple message to HTTP responses
            //that would not otherwise have a body, such as 404 - Not Found
            //responses.
            //app.UseStatusCodePages();

            
            /*
            // This is terminal middleware, will reverse himself
            // and all the commands after him will not execute
            app.Run(async(context) => {
                   await context.Response.WriteAsync("Hello, World");
            });*/
            //app middleware to default files on wwwroot folder.
            //It is need to be written before app.UseStaticFiles();
            app.UseDefaultFiles();

           
            //Adds middleware for redirecting HTTP Requests to HTTPS.
            app.UseHttpsRedirection();
            //This middleware enables support for serving static files from
            //the wwwroot folder such as HTML/CSS/JS files, Photos and more
            app.UseStaticFiles();
            //Adds the CookiePolicy middleware handler to the specified IApplicationBuilder, which enables cookie policy capabilities.
            app.UseCookiePolicy();
            //Adds the Authentication mddleware to the specified IApplicationBuilder, which enables authentication capabilities.
            app.UseAuthentication();
            //Allows the session system to
            //automatically associate requests with sessions when they arrive from the client.
            app.UseSession();
            //This extension method enables ASP.NET Core MVC.
            //Adds MVC to the IApplicationBuilder request execution pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Default",
                    template: "{controller=Home}/{action=ListCountry}/{Country?}"
                );

            
                routes.MapRoute(
                    null,
                    "Page{songPage:int}",
                    new
                    {
                        controller = "Home",
                        action = "ListCountry", songPage = 1
                    }
                );

                routes.MapRoute(
                    null,
                    "{country}",
                    new
                    {
                        controller = "Home",
                        action = "ListCountry", songPage = 1
                    }
                );

                routes.MapRoute(
                    null,
                    "",
                    new
                    {
                        controller = "Home", action = "ListCountry",
                        songPage = 1
                    });

                routes.MapRoute(
                    null,
                    "",
                    new
                    {
                        controller = "User",
                        action = "CancelLogin",
                        songPage = 1
                    });


            });

            //SeedData.EnsurePopulated(app);
            //IdentitySeedData.EnsurePopulated(app);
        }

        /*private RequestDelegate async(object context)
        {
            throw new NotImplementedException();
        }*/
    }
}