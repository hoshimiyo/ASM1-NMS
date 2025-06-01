using Helpers;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class GuestService : IGuestService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/api/Guest/"; // Adjust the base URL as needed

        public GuestService(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }


        public async Task<IEnumerable<NewsArticleViewModel>> GetArticlesWithActiveCategories()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "Index");

            var result = await response.ReadContentAsync<IEnumerable<NewsArticleViewModel>>();

            return result;
        }
    }
}
