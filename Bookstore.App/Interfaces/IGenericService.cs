using System.Collections.Generic;
using System.Threading.Tasks;
using Bookstore.App.Models;

namespace Bookstore.App.Interfaces
{
    public interface IGenericService<T> where T : BaseModel
    {
        Task<IEnumerable<T>> GetAllAsync(); // Retrieve all entities
        Task<T> GetByIdAsync(int id); // Retrieve an entity by ID
        Task<T> CreateAsync(T entity); // Add a new entity
        Task<T> UpdateAsync(T entity); // Update an existing entity
        Task<bool> DeleteAsync(int id); // Delete an entity by ID
    }
}
