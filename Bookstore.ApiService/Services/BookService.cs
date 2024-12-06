using System.Text.Json;
using Bookstore.ApiService.Interfaces;
using Bookstore.ApiService.Models;
using StackExchange.Redis;

namespace Bookstore.ApiService.Services
{
    public class BookService : IBookService
    {
        private readonly IDatabase _database;
        private readonly List<Book> _books = new List<Book>(); // In-memory collection for demonstration

        public BookService(IDatabase database)
        {
            _database = database;
        }

        //private static readonly List<Book> _books = new()
        //{
        //    new Book { Id = Guid.NewGuid(), Title = "The Lord of the Rings", Author = "J.R.R. Tolkien", ISBN = "9780261102354", Price = 29.99M, Genre = "Fantasy" },
        //    new Book { Id = Guid.NewGuid(), Title = "The Hitchhiker's Guide to the Galaxy", Author = "Douglas Adams", ISBN = "9780345391803", Price = 12.99M, Genre = "Science Fiction" },
        //    new Book { Id = Guid.NewGuid(), Title = "Pride and Prejudice", Author = "Jane Austen", ISBN = "9780141439518", Price = 9.99M, Genre = "Romance" }
        //};

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            await Task.Delay(100); // Simulate asynchronous operation
            return _books.ToList();
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            // Attempt to retrieve book from Redis
            var cachedBookJson = await _database.StringGetAsync(id.ToString());
            if (!string.IsNullOrEmpty(cachedBookJson))
            {
                return JsonSerializer.Deserialize<Book>(cachedBookJson);
            }

            // If not in Redis, fetch from in-memory collection
            var book = _books.FirstOrDefault(b => b.Id == id);

            // Cache the retrieved book in Redis
            if (book != null)
            {
                await _database.StringSetAsync(id.ToString(), JsonSerializer.Serialize(book), TimeSpan.FromMinutes(10));
            }

            return book;
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            _books.Add(book);

            // Cache the newly created book
            await _database.StringSetAsync(book.Id.ToString(), JsonSerializer.Serialize(book), TimeSpan.FromMinutes(10));

            return book;
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook == null)
            {
                return null;
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.ISBN = book.ISBN;
            existingBook.Price = book.Price;
            existingBook.Genre = book.Genre;

            // Invalidate the cache for the updated book
            await _database.KeyDeleteAsync(book.Id.ToString());

            // Update the cached book
            await _database.StringSetAsync(book.Id.ToString(), JsonSerializer.Serialize(existingBook), TimeSpan.FromMinutes(10));

            return existingBook;
        }

        public async Task DeleteBookAsync(Guid id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _books.Remove(book);

                // Invalidate the cache for the deleted book
                await _database.KeyDeleteAsync(id.ToString());
            }
        }
    }
}
