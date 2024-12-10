using Bookstore.App.Interfaces;
using Bookstore.App.Models;
using Bookstore.App.Services;

namespace Bookstore.App
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HttpClient _httpClient;

        public UnitOfWork(HttpClient httpClient)
        {
            _httpClient = httpClient;

            Books = new GenericService<Book>(_httpClient, "books");
            Customers = new GenericService<Customer>(_httpClient, "customers");
            Orders = new GenericService<Order>(_httpClient, "orders");
        }

        public IGenericService<Book> Books { get; }
        public IGenericService<Customer> Customers { get; }
        public IGenericService<Order> Orders { get; }

        public Task SaveChangesAsync()
        {
            // If you have transactional backend support, coordinate save operations here
            return Task.CompletedTask;
        }
    }
}
