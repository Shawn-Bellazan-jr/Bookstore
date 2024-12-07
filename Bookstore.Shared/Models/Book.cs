using System.ComponentModel.DataAnnotations;

namespace Bookstore.Shared.Models
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "ISBN is required.")]
        public string ISBN { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        public string Genre { get; set; }
    }
}
