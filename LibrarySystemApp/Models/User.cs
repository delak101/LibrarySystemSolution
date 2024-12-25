using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LibrarySystemApp.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)] // Ensure name length is reasonable
        public string Name { get; set; }
        
        [Required]
        [MaxLength(256)] // Ensures compatibility with SQL Server
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)] // Ensure password security
        public string PasswordHash { get; set; }

        public UserRole Role { get; set; }

        [MaxLength(100)] // Limit department name length
        public string Department { get; set; }  // GENERAL, IT, CS, IS, DS, AI, GIS, BIO

        [Required]
        [Phone]
        public string? Phone { get; set; } // Changed to string to support formatting

        public int Year { get; set; } // Nullable to handle non-student roles

        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Borrow> Borrows { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}

public enum UserRole
{
    Admin = 0,
    Student = 1
}