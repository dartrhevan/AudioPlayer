
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

        public static string ConnectionString { get; set; } =
            "User ID=postgres;Password=android;Host=localhost;Port=5432;Database=playerAuth;Pooling=true;SSL Mode=Prefer;Trust Server Certificate=True";//TODO: initialize

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(ConnectionString);
            optionsBuilder.UseNpgsql(ConnectionString);
            //UseNpgsql
            base.OnConfiguring(optionsBuilder);
        }
    }
}