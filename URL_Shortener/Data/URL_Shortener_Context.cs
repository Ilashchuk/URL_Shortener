using Microsoft.EntityFrameworkCore;
using URL_Shortener.Models;

namespace URL_Shortener.Data
{
    public class URL_Shortener_Context : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Url> Urls { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public URL_Shortener_Context(DbContextOptions<URL_Shortener_Context> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "44bohdan44@gmail.com";
            string adminPassword = "123456";

            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
