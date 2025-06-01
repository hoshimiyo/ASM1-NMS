using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/api/Categories/"; // Adjust the base URL as needed

        public CategoryService(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task CreateCategoryAsync(CategoryViewModel category)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl + "Create", category);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(BaseUrl + "DeleteConfirmed?id=" + id);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "GetAllCategories");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<CategoryViewModel>>();
            return result;
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(BaseUrl + "Details/" + id);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CategoryViewModel>();
            return result;
        }

        public async Task UpdateCategoryAsync(int id, CategoryViewModel category)
        {
            var response = await _httpClient.PutAsJsonAsync(BaseUrl + "Edit/" + id, category);
            response.EnsureSuccessStatusCode();
        }
    }
}
