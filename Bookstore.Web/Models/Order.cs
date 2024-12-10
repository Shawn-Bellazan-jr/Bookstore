namespace Bookstore.Web.Models
{
    public class Order : BaseModel
    {
        public string CustomerName { get; set; }
        public decimal Total { get; set; }
    }
}
