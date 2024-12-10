using Bookstore.Web.Interfaces;
using Bookstore.Web.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Bookstore.Web.Services
{
    public class GenericService<T> : IGenericService<T> where T : BaseModel
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _endpoint;

        public GenericService(IHttpClientFactory httpClientFactory, string endpoint)
        {
            _endpoint = endpoint;
            // Using named HttpClient for Bookstore API
            _httpClient = httpClientFactory.CreateClient("BookstoreApi");
            _httpClient.BaseAddress = new("https+http://apiservice/");
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
