using Helpers;
using NMS_API_FE.Helpers;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        public const string BaseUrl = "/odata/Categories"; // Adjust the base URL as needed

        public CategoryService(HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));    
        }

        public async Task CreateCategoryAsync(CategoryViewModel category)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Post, BaseUrl, "", category);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Delete, BaseUrl, $"({id})");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, BaseUrl, "?$expand=ParentCategory");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<ODataResponse<CategoryViewModel>>();
            return result.Value;
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(int id)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, BaseUrl, $"({id})?$expand=ParentCategory");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<CategoryViewModel>();
            return result;
        }

        public async Task UpdateCategoryAsync(int id, CategoryViewModel category)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Put, BaseUrl, $"({id})?$expand=ParentCategory", category);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
