

using System.Net.Http;
using Bookstore.Web.Interfaces;
using Bookstore.Web.Models;
using Bookstore.Web.Services;

namespace Bookstore.Web
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UnitOfWork(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory; 

            Books = new BookService(_httpClientFactory, "books");
            Customers = new CustomerService(_httpClientFactory, "customers");
            Orders = new OrderService(_httpClientFactory, "orders");
        }

        public IBookService Books { get; }
        public ICustomerService Customers { get; }
        public IOrderService Orders { get; }

        public Task SaveChangesAsync()
        {
            // If you have transactional backend support, coordinate save operations here
            return Task.CompletedTask;
        }
    }
}
