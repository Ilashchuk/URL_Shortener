using Microsoft.EntityFrameworkCore;
using URL_Shortener.Models;

namespace URL_Shortener.Data
{
    public class URL_Shortener_Context : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Url> Urls { get; set; } = null!;
        public URL_Shortener_Context(DbContextOptions<URL_Shortener_Context> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
