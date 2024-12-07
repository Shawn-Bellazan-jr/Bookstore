using Bookstore.Shared.Models;
using Bookstore.Web.Interfaces;
using System.Net.Http.Json;

namespace Bookstore.Web.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;

        public BookService(IHttpClientFactory httpClientFactory)
        {
            // Using named HttpClient for Bookstore API
            _httpClient = httpClientFactory.CreateClient("BookstoreApi");
            _httpClient.BaseAddress = new("https+http://apiservice");
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/books");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<IEnumerable<Book>>() ?? Enumerable.Empty<Book>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching books: {ex.Message}");
                throw;
            }
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/books/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Book>()
                       ?? throw new Exception($"Book with ID {id} not found.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching book with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/books", book);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Book>()
                       ?? throw new Exception("Failed to create the book.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating book: {ex.Message}");
                throw;
            }
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/books/{book.Id}", book);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Book>()
                       ?? throw new Exception("Failed to update the book.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error updating book with ID {book.Id}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteBookAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/books/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting book with ID {id}: {ex.Message}");
                throw;
            }
        }
    }
}
