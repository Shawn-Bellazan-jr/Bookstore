namespace Bookstore.App.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; } // Common identifier for all entities
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
