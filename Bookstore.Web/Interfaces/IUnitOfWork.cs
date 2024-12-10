using Bookstore.Web.Models;

namespace Bookstore.Web.Interfaces
{
    public interface IUnitOfWork
    {
        IBookService Books { get; }
        ICustomerService Customers { get; }
        IOrderService Orders { get; }
        Task SaveChangesAsync(); // Optional: If you want to batch save changes
    }
}
