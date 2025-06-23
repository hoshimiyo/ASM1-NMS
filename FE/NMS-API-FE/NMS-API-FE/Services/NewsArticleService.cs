using Helpers;
using NMS_API_FE.DTOs;
using NMS_API_FE.Helpers;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class NewsArticleService : INewsArticlesService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/odata/NewsArticles"; // Adjust the base URL as needed
        public const string SearchUrl = "/api/NewsSearch/"; 
        private readonly IHttpContextAccessor _contextAccessor;

        public NewsArticleService(HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
        }
        public async Task CreateNewsArticleAsync(NewsArticleCreateDTO dto, int userId)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Post, BaseUrl, $"?userId={userId}", dto);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteNewsArticleAsync(string id)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Delete, BaseUrl, $"('{id.ToString()}')");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<NewsArticleViewModel> GetNewsArticleByIdAsync(string id)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, BaseUrl, $"('{id.ToString()}')?$expand=Category,CreatedBy,UpdatedBy,NewsTags($expand=Tag)");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<NewsArticleViewModel>();
            return result ?? throw new InvalidOperationException("Article not found");
        }

        public async Task<IEnumerable<NewsArticleViewModel>> GetNewsArticlesAsync()
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, BaseUrl, "?$expand=Category,CreatedBy,UpdatedBy,NewsTags($expand=Tag)");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.Content);
            var result = await response.ReadContentAsync<ODataResponse<NewsArticleViewModel>>();
            return result.Value ?? Enumerable.Empty<NewsArticleViewModel>();
        }

        public async Task UpdateNewsArticleAsync(string id, NewsArticleUpdateDTO dto, int userId)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Put, BaseUrl, $"('{id.ToString()}')?userId={userId}", dto);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<NewsArticleViewModel>> SearchNewsArticles(string searchTerm, int categoryId, int tagId)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, SearchUrl, $"Search?searchTerm={Uri.EscapeDataString(searchTerm)}&categoryId={categoryId}&tagId={tagId}");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<IEnumerable<NewsArticleViewModel>>();
            return result ?? Enumerable.Empty<NewsArticleViewModel>();
        }
    }
}
