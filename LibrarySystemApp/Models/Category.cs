using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] // Limit category name length
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; } // Many-to-many relationship with Books
    }
}