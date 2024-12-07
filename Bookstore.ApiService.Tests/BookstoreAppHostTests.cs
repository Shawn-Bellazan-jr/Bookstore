using Aspire.Hosting;
using Bookstore.Shared.Models;
using Projects;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class BookstoreAppHostTests : IAsyncLifetime
{
    private DistributedApplication _app;
    private HttpClient _client;

    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Bookstore_AppHost>();
        _app = await appHost.BuildAsync();
        await _app.StartAsync();
        _client = _app.CreateHttpClient("apiservice");

        // Seed test data
        await SeedTestDataAsync();
    }

    private async Task SeedTestDataAsync()
    {
        var books = new List<Book>
        {
            new Book
            {
                Id = Guid.Parse("b47d7c44-7a3d-4d0f-a529-d5a0d8b5731f"),
                Title = "The Lord of the Rings",
                Author = "J.R.R. Tolkien",
                ISBN = "1234567890",
                Price = 29.99m,
                Genre = "Fantasy"
            },
            new Book
            {
                Id = Guid.Parse("c3b8b4d9-95af-4a9c-bd74-5d2cbe6d9f4d"),
                Title = "The Hitchhiker's Guide to the Galaxy",
                Author = "Douglas Adams",
                ISBN = "0987654321",
                Price = 15.99m,
                Genre = "Science Fiction"
            }
        };

        foreach (var book in books)
        {
            var response = await _client.PostAsJsonAsync("/books", book);
            response.EnsureSuccessStatusCode();
        }
    }

    public async Task DisposeAsync()
    {
        if (_app != null)
        {
            await _app.DisposeAsync();
        }
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenBooksExist()
    {
        // Act
        var response = await _client.GetAsync("/books");

        // Assert
        response.EnsureSuccessStatusCode();
        var books = await response.Content.ReadFromJsonAsync<List<Book>>();
        Assert.NotNull(books);
        Assert.NotEmpty(books);
    }

    [Theory]
    [InlineData("b47d7c44-7a3d-4d0f-a529-d5a0d8b5731f", "The Lord of the Rings")]
    [InlineData("c3b8b4d9-95af-4a9c-bd74-5d2cbe6d9f4d", "The Hitchhiker's Guide to the Galaxy")]
    public async Task GetBookByIdAsync_ReturnsCorrectBook(string bookId, string expectedTitle)
    {
        var guid = Guid.Parse(bookId);

        // Act
        var response = await _client.GetAsync($"/books/{guid}");

        // Assert
        response.EnsureSuccessStatusCode();
        var book = await response.Content.ReadFromJsonAsync<Book>();
        Assert.NotNull(book);
        Assert.Equal(expectedTitle, book.Title);
    }

    [Fact]
    public async Task Post_CreatesNewBook()
    {
        // Arrange
        var newBook = new Book
        {
            Title = "The Hobbit",
            Author = "J.R.R. Tolkien",
            ISBN = "9780261102361",
            Price = 19.99M,
            Genre = "Fantasy"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/books", newBook);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdBook = await response.Content.ReadFromJsonAsync<Book>();
        Assert.NotNull(createdBook);
        Assert.NotEqual(Guid.Empty, createdBook.Id);
        Assert.Equal(newBook.Title, createdBook.Title);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenBookTitleIsMissing()
    {
        // Arrange
        var invalidBook = new Book
        {
            Author = "J.R.R. Tolkien",
            ISBN = "9780261102361",
            Price = 19.99M,
            Genre = "Fantasy"
        }; // Title is missing

        // Act
        var response = await _client.PostAsJsonAsync("/books", invalidBook);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_UpdatesExistingBook()
    {
        // Arrange
        var updatedBook = new Book
        {
            Id = Guid.Parse("b47d7c44-7a3d-4d0f-a529-d5a0d8b5731f"),
            Title = "Updated Title",
            Author = "Updated Author",
            ISBN = "Updated ISBN",
            Price = 24.99M,
            Genre = "Updated Genre"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/books/{updatedBook.Id}", updatedBook);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Delete_DeletesExistingBook()
    {
        // Act
        var deleteResponse = await _client.DeleteAsync($"/books/{Guid.Parse("b47d7c44-7a3d-4d0f-a529-d5a0d8b5731f")}");

        // Assert
        deleteResponse.EnsureSuccessStatusCode();

        // Verify that the book was deleted
        var getResponse = await _client.GetAsync($"/books/{Guid.Parse("b47d7c44-7a3d-4d0f-a529-d5a0d8b5731f")}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenBookIdIsMissing()
    {
        // Arrange
        // A book object without an Id
        var updatedBook = new Book
        {
            Title = "Updated Title",
            Author = "Updated Author",
            ISBN = "Updated ISBN",
            Price = 24.99M,
            Genre = "Updated Genre"
        };

        // Act
        // Provide a route parameter, but the body lacks the required Id
        var response = await _client.PutAsJsonAsync($"/books/{Guid.NewGuid()}", updatedBook);

        // Assert
        // Expect the response to indicate a bad request due to the missing Id in the body
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }


    [Fact]
    public async Task Put_ReturnsNotFound_WhenUpdatingNonexistentBook()
    {
        // Arrange
        var nonExistentBookId = Guid.NewGuid();
        var updatedBook = new Book
        {
            Id = nonExistentBookId,
            Title = "Updated Title",
            Author = "Updated Author",
            ISBN = "Updated ISBN",
            Price = 24.99M,
            Genre = "Updated Genre"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/books/{nonExistentBookId}", updatedBook);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenDeletingNonexistentBook()
    {
        // Arrange
        var nonExistentBookId = Guid.NewGuid(); // Generate a new Guid that does not match any existing book

        // Act
        var response = await _client.DeleteAsync($"/books/{nonExistentBookId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // Add a test to verify the behavior when posting an empty request body
    [Fact]
    public async Task Post_ReturnsBadRequest_WithEmptyRequestBody()
    {
        // Act
        // Send a POST request with an empty content
        var response = await _client.PostAsync("/books", new StringContent("", System.Text.Encoding.UTF8, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}