using Bookstore.App.Models;

namespace Bookstore.App.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericService<Book> Books { get; }
        IGenericService<Customer> Customers { get; }
        IGenericService<Order> Orders { get; }
        Task SaveChangesAsync(); // Optional: If you want to batch save changes
    }
}
