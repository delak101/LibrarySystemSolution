using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibrarySystemApp.Models
{
    public class Borrow
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; } // Student who borrowed

        [Required]
        public int BookId { get; set; } // Book being borrowed

        [Required]
        public DateTime BorrowDate { get; set; } = DateTime.Today; // Default to now

        [Required]
        public DateTime DueDate { get; set; } // Admin sets due date

        public DateTime? ReturnDate { get; set; } // Null if not returned yet

        [Required]
        public BorrowStatus Status { get; set; } = BorrowStatus.Pending; // Default: Pending

        // Navigation properties
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public Book? Book { get; set; }
    }

    public enum BorrowStatus
    {
        Pending,  // Student requested
        Approved, // Admin approved
        Denied,   // Admin denied
        Returned  // Book returned
    }
}