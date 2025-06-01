using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class NewsTagService : INewsTagService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/api/NewsTag/"; // Adjust the base URL as needed

        public NewsTagService(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task AddNewsTagAsync(string NewsArticleId, int TagId)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl + "AddNewsTag", new { NewsArticleId, TagId });
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<TagViewModel>> GetTagsOfArticleAsync(string NewsArticleId)
        {
            var response = await _httpClient.GetAsync(BaseUrl + "GetTagsOfArticleAsync/" + NewsArticleId);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<TagViewModel>>();
            return result ?? Enumerable.Empty<TagViewModel>();
        }

        public async Task<IEnumerable<NewsArticleViewModel>> GetArticlesFromTagAsync(int TagId)
        {
            var response = await _httpClient.GetAsync(BaseUrl + "GetArticlesFromTag/" + TagId);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<NewsArticleViewModel>>();
            return result ?? Enumerable.Empty<NewsArticleViewModel>();
        }

        public async Task DeleteNewsTagAsync(string NewsArticleId, int TagId)
        {
            var response = await _httpClient.DeleteAsync(BaseUrl + "DeleteNewsTag/" + NewsArticleId + "/" + TagId);
            response.EnsureSuccessStatusCode();
        }
    }
}
