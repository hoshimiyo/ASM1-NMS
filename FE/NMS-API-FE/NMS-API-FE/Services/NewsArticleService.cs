using Helpers;
using NMS_API_FE.DTOs;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class NewsArticleService : INewsArticlesService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/api/NewsArticles/"; // Adjust the base URL as needed

        public NewsArticleService(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task CreateNewsArticleAsync(NewsArticleCreateDTO dto, int userId)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl + "Create/" + userId, dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteNewsArticleAsync(string id)
        {
            var response = await _httpClient.DeleteAsync(BaseUrl + "Delete/" + id);
            response.EnsureSuccessStatusCode();
        }

        public async Task<NewsArticleViewModel> GetNewsArticleByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync(BaseUrl + "Details/" + id);
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<NewsArticleViewModel>();
            return result ?? throw new InvalidOperationException("Article not found");
        }

        public async Task<IEnumerable<NewsArticleViewModel>> GetNewsArticlesAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "GetAllArticles");
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<IEnumerable<NewsArticleViewModel>>();
            return result ?? Enumerable.Empty<NewsArticleViewModel>();
        }

        public async Task UpdateNewsArticleAsync(string id, NewsArticleUpdateDTO dto, int userId)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}Edit/{id}/{userId}", dto);
            Console.WriteLine("API URL "+ BaseUrl + "Edit/" + id);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<NewsArticleViewModel>> SearchNewsArticles(string searchTerm, int categoryId, int tagId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}Search?searchTerm={Uri.EscapeDataString(searchTerm)}&categoryId={categoryId}&tagId={tagId}");
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<IEnumerable<NewsArticleViewModel>>();
            return result ?? Enumerable.Empty<NewsArticleViewModel>();
        }
    }
}
