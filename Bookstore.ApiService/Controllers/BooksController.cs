using Bookstore.ApiService.Interfaces;
using Bookstore.ApiService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.ApiService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Gets a list of all books.
        /// </summary>
        /// <returns>A list of books.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        /// <summary>
        /// Gets a book by ID.
        /// </summary>
        /// <param name="id">The ID of the book.</param>
        /// <returns>The book with the specified ID, or NotFound if not found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        /// <summary>
        /// Creates a new book.
        /// </summary>
        /// <param name="book">The book to create.</param>
        /// <returns>The created book.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBook(Book book)
        {
            if (book == null)
            {
                return BadRequest("Request body cannot be empty.");
            }

            // Perform additional validation if necessary (e.g., check required fields)
            if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
            {
                return BadRequest("Book must have a title and an author.");
            }

            var createdBook = await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="book">The updated book information.</param>
        /// <returns>NoContent if the book was updated successfully, BadRequest if the IDs do not match, or NotFound if the book was not found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, Book book)
        {
            // Validate that the route id matches the book's id
            if (id == Guid.Empty || id != book.Id)
            {
                return BadRequest("The book Id in the URL must match the Id in the body.");
            }

            var updatedBook = await _bookService.UpdateBookAsync(book);
            if (updatedBook == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes a book by ID.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>NoContent if the book was deleted successfully.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            // Check if the book exists before attempting deletion
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound(); // Return 404 if the book does not exist
            }

            // Proceed with deletion if the book exists
            await _bookService.DeleteBookAsync(id);
            return NoContent(); // Return 204 if the deletion is successful
        }

    }
}