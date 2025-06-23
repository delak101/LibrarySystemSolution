using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string? pic { get; set; }

        [Required]
        [MaxLength(100)] // Limit author name length
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; } // Many-to-many relationship with Books
    }
}
