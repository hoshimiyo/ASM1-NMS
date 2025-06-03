using Helpers;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class LecturerService : ILecturerService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/api/Lecturer/"; // Adjust the base URL as needed

        public LecturerService(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<NewsArticleViewModel>> GetNewsArticles()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "GetArticlesWithActiveCategories");
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync< IEnumerable<NewsArticleViewModel>>();
            return result;
        }

        public async Task<NewsArticleViewModel> GetNewsArticleById(string id)
        {
            var response = await _httpClient.GetAsync(BaseUrl + "Details/" + id);
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<NewsArticleViewModel>();
            return result;
        }
    }
}
