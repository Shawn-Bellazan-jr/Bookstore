using Bookstore.Web.Interfaces;
using Bookstore.Web.Models;

namespace Bookstore.Web.Services
{
    public class OrderService : GenericService<Order>, IOrderService
    {
        public OrderService(IHttpClientFactory httpClientFactory, string endpoint) : base(httpClientFactory, endpoint)
        {
        }
    }
}
