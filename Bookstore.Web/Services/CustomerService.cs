using Bookstore.Web.Interfaces;
using Bookstore.Web.Models;

namespace Bookstore.Web.Services
{
    public class CustomerService : GenericService<Customer>, ICustomerService
    {
        public CustomerService(IHttpClientFactory httpClientFactory, string endpoint) : base(httpClientFactory, endpoint)
        {
        }
    }
}
