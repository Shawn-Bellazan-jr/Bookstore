namespace Bookstore.App.Models
{
    public class Book
    {
        public int Id { get; set; } // Unique identifier for the book
        public string Title { get; set; } = string.Empty; // Title of the book
        public string Author { get; set; } = string.Empty; // Author's name
        public decimal Price { get; set; } = 0.0m; // Price of the book

        // Additional properties can be added as needed
        public string Genre { get; set; } = string.Empty; // Optional: Genre of the book
        public DateTime PublishedDate { get; set; } = DateTime.Now; // Optional: Publication date
    }
}
