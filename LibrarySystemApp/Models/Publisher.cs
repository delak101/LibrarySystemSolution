using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] // Limit publisher name length
        public string Name { get; set; }

        [MaxLength(500)] // Limit address length
        public string Address { get; set; }

        public ICollection<Book> Books { get; set; } // One-to-many relationship with Books
    }
}
