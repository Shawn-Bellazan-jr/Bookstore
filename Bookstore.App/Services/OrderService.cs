using Bookstore.App.Interfaces;
using Bookstore.App.Models;

namespace Bookstore.App.Services
{
    public class OrderService : GenericService<Order>, IOrderService
    {
        public OrderService(HttpClient httpClient, string endpoint) : base(httpClient, endpoint)
        {
        }
    }
}
