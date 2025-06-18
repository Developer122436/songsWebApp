using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using SongsProject.Models; // for ApplicationDbContext, if needed
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Configure NLog for logging
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

// Add services (calls Startup.cs methods)
var startup = new SongsProject.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// ✅ Seed admin user and role
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await EnsureAdminExistsAsync(services); // <-- method defined below
}

// Configure middleware pipeline (calls Startup.cs method)
startup.Configure(app, app.Environment);

app.Run();


// ✅ Admin Role Seeder
async Task EnsureAdminExistsAsync(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string adminEmail = "dimaspektor12@gmail.com";
    string roleName = "Admin"; // or "InformationTechnology" if you prefer

    // Create role if it doesn't exist
    if (!await roleManager.RoleExistsAsync(roleName))
    {
        await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    // Assign user to role
    var user = await userManager.FindByEmailAsync(adminEmail);
    if (user != null && !await userManager.IsInRoleAsync(user, roleName))
    {
        await userManager.AddToRoleAsync(user, roleName);
    }

}
