using Bookstore.App.Interfaces;
using Bookstore.App.Models;
using System.Net.Http.Json;

namespace Bookstore.App.Services
{
    public class GenericService<T> : IGenericService<T> where T : BaseModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;

        public GenericService(HttpClient httpClient, string endpoint)
        {
            _httpClient = httpClient;
            _endpoint = endpoint;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<T>>(_endpoint);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<T>($"{_endpoint}/{id}");
        }

        public async Task<T> CreateAsync(T entity)
        {
            var response = await _httpClient.PostAsJsonAsync(_endpoint, entity);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/{entity.Id}", entity);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
