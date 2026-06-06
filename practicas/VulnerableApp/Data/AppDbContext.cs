using Microsoft.EntityFrameworkCore;
using VulnerableApp.Models;

namespace VulnerableApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.Balance).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", PasswordHash = "$2a$11$K.xV/6E6D8.lV4Yp1r4Y.O/C.v7v7v7v7v7v7v7v7v7v7v7v7v7v7", Email = "admin@test.com", Balance = 1000m, CreatedAt = new DateTime(2024, 1, 1) },
                new User { Id = 2, Username = "user1", PasswordHash = "$2a$11$e/r1v7v7v7v7v7v7v7v7v.O/C.v7v7v7v7v7v7v7v7v7v7v7v7v7v", Email = "user@test.com", Balance = 500m, CreatedAt = new DateTime(2024, 1, 1) },
                new User { Id = 3, Username = "user2", PasswordHash = "$2a$11$w/q1v7v7v7v7v7v7v7v7v.O/C.v7v7v7v7v7v7v7v7v7v7v7v7v7v", Email = "user2@test.com", Balance = 750m, CreatedAt = new DateTime(2024, 1, 1) }
            );
        }
    }
}