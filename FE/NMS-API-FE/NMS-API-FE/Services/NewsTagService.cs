using Helpers;
using NMS_API_FE.Helpers;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class NewsTagService : INewsTagService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/odata/NewsTags"; // Adjust the base URL as needed
        public const string NewsTagRelationsUrl = "/api/NewsTagRelations/";
        private readonly IHttpContextAccessor _contextAccessor;

        public NewsTagService(HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
        }

        public async Task AddNewsTagAsync(string NewsArticleId, int TagId)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Post, BaseUrl, "", new { NewsArticleId, TagId});
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<TagViewModel>> GetTagsOfArticleAsync(string NewsArticleId)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, NewsTagRelationsUrl, $"GetTagsOfArticleAsync/{NewsArticleId}");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<ODataResponse<TagViewModel>>();
            return result.Value ?? Enumerable.Empty<TagViewModel>();
        }

        public async Task<IEnumerable<NewsArticleViewModel>> GetArticlesFromTagAsync(int TagId)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, NewsTagRelationsUrl, $"GetArticlesFromTag/{TagId}");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<ODataResponse<NewsArticleViewModel>>();
            return result.Value ?? Enumerable.Empty<NewsArticleViewModel>();
        }

        public async Task DeleteNewsTagAsync(string NewsArticleId, int TagId)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Delete, BaseUrl,$"(NewsArticleId='{NewsArticleId}',TagId={TagId})");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
