using Helpers;
using NMS_API_FE.Helpers;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class LecturerService : ILecturerService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/odata/NewsArticles"; // Adjust the base URL as needed
        private readonly IHttpContextAccessor _contextAccessor;

        public LecturerService(HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
        }

        public async Task<IEnumerable<NewsArticleViewModel>> GetNewsArticles()
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, BaseUrl, "");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<ODataResponse<NewsArticleViewModel>>();
            return result.Value;
        }

        public async Task<NewsArticleViewModel> GetNewsArticleById(string id)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, BaseUrl, $"({id})");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<NewsArticleViewModel>();
            return result;
        }
    }
}
