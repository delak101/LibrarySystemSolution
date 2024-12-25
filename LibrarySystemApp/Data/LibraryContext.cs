using LibrarySystemApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<User?> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Borrow> Borrows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User entity configuration
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Phone)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(u => u.Department)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Year)
                .IsRequired(false);

            // Favorite entity configuration
            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.UserId, f.BookId });

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Book)
                .WithMany(b => b.Favorites)
                .HasForeignKey(f => f.BookId);

            // Book entity configuration
            modelBuilder.Entity<Book>()
                .Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Book>()
                .HasIndex(b => b.Name);

            modelBuilder.Entity<Book>()
                .HasIndex(b => b.Department);

            modelBuilder.Entity<Book>()
                .Property(b => b.Image)
                .IsRequired(false);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relationships and many-to-many setups
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Categories)
                .WithMany(c => c.Books)
                .UsingEntity(j => j.ToTable("BookCategories"));

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books)
                .UsingEntity(j => j.ToTable("AuthorBooks"));

            // Review entity configuration
            modelBuilder.Entity<Review>()
                .HasIndex(r => new { r.UserId, r.BookId });

            // Borrow entity configuration
            modelBuilder.Entity<Borrow>()
                .HasIndex(b => new { b.UserId, b.BookId, b.BorrowDate });
        }
    }
}
