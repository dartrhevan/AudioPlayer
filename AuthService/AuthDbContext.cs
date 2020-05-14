
using Microsoft.EntityFrameworkCore;

namespace AuthService
{
    public class AuthDbContext : DbContext

    {

        public AuthDbContext()
        {

            Database.EnsureCreated();
        }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public static string ConnectionString { get; set; }//TODO: initialize

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(ConnectionString);
            optionsBuilder.UseNpgsql(ConnectionString);
            //UseNpgsql
            base.OnConfiguring(optionsBuilder);
        }
    }
}