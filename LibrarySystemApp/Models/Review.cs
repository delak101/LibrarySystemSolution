using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Required]
        [Range(1, 5)] // Rating between 1 and 5
        public int Rating { get; set; }

        [MaxLength(1000)] // Limit comment length
        public string Comment { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; } // Ensure review has a date
    }
}