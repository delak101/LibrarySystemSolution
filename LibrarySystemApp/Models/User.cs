using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibrarySystemApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? ProfilePicture { get; set; }
        [MaxLength(100)] // Ensure name length is reasonable
        public required string Name { get; set; }

        [MaxLength(256)] // Ensures compatibility with SQL Server
        [EmailAddress]
        public required string Email { get; set; }
        
        [MaxLength(256)] // Ensures compatibility with SQL Server
        [EmailAddress]
        public string? StudentEmail { get; set; }
        
        [MinLength(8)] // Ensure password security
        public required string PasswordHash { get; set; }

        public string Role { get; set; } = "User"; // Default role is User, can be Admin

        [MaxLength(100)] // Limit department name length
        public required string Department { get; set; } // GENERAL, IT, CS, IS, DS, AI, GIS, BIO

        [Phone] 
        public required string Phone { get; set; } // Changed to string to support formatting

        public required int NationalId { get; set; }
        public required int Year { get; set; }
        
        public required bool TermsAccepted { get; set; }
        
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        [JsonIgnore]
        public ICollection<Favorite>? Favorites { get; set; }
        [JsonIgnore]
        public ICollection<Borrow>? Borrows { get; set; }
        [JsonIgnore]
        public ICollection<Review>? Reviews { get; set; }
    }
}
