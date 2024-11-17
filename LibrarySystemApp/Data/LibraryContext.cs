using LibrarySystemApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Data;

public class LibraryContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256) // Ensures compatibility with SQL Server
            .HasColumnType("nvarchar(256)"); // Explicitly set the column type

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

    }
}