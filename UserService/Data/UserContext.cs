using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data;

public class UserContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
    }
}