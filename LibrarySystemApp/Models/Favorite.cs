using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.Models
{
    public class Favorite
    {
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}