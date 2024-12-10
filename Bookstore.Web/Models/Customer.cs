namespace Bookstore.Web.Models
{
    public class Customer : BaseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
