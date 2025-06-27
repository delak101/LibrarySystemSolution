using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibrarySystemApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        [MaxLength(500)] // Ensure the name length is reasonable
        public required string Name { get; set; }
        
        [MaxLength(1000)] // Limit description length
        public string? Description { get; set; }

        [MaxLength(50)] // Ensure shelf identifier is concise
        public required string Shelf { get; set; }

        public bool IsAvailable { get; set; } = true; // Default to available

        [MaxLength(100)]
        public string? Department { get; set; }  // The department using the book (IT, CS, etc.)

        public int? AssignedYear { get; set; } // Represents the year the book is assigned to (1st, 2nd, 3rd, 4th)

        public string? Image { get; set; } // URL or base64 for book image

        public Publisher? Publisher { get; set; }
        public int? PublisherId { get; set; }

        [JsonIgnore]
        public ICollection<Category>? Categories { get; set; }
        [JsonIgnore]
        public ICollection<Author>? Authors { get; set; }
        [JsonIgnore]
        public ICollection<Favorite>? Favorites { get; set; }
        [JsonIgnore]
        public ICollection<Review>? Reviews { get; set; }
        [JsonIgnore]
        public ICollection<Borrow>? Borrows { get; set; }
    }
}