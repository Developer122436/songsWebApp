using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SongsProject.Models.ViewModels;

namespace SongsProject.Models
{
    // Defining a DbContext in the project is what allows us 
    // to persist data to our database and populate data from the database. 
    public class ApplicationDbContext : IdentityDbContext
    {
        //Next, let’s add a constructor that takes in DbContextOptions as a parameter. the options that are passed in will 
        //be used to pass the connection string when we instantiate it at startup.
        //We pass a configuration information.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
            
        }

    }
}