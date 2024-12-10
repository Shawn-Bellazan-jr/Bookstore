using Bookstore.Web.Interfaces;
using Bookstore.Web.Models;

namespace Bookstore.Web.Services
{

    public class BookService : GenericService<Book>, IBookService
    {
        public BookService(IHttpClientFactory httpClientFactory, string endpoint) : base(httpClientFactory, endpoint)
        {
        }
    }
}
