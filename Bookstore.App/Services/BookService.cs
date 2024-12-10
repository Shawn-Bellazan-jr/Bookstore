using Bookstore.App.Interfaces;
using Bookstore.App.Models;

namespace Bookstore.App.Services
{
    public class BookService : GenericService<Book>, IBookService
    {


        // Add any additional Book-specific methods here
        public BookService(HttpClient httpClient, string endpoint) : base(httpClient, endpoint)
        {
        }
    }
}
