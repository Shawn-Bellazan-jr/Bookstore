using Bookstore.App.Interfaces;
using Bookstore.App.Models;
using Bookstore.App.Pages;

namespace Bookstore.App.Services
{
    public class CustomerService : GenericService<Customer>, ICustomerService
    {
        public CustomerService(HttpClient httpClient, string endpoint) : base(httpClient, endpoint)
        {
        }
    }
}
