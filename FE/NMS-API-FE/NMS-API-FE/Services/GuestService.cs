using Helpers;
using NMS_API_FE.Helpers;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class GuestService : IGuestService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/odata/NewsArticles"; // Adjust the base URL as needed
        private readonly IHttpContextAccessor _contextAccessor;

        public GuestService(HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
        }


        public async Task<IEnumerable<NewsArticleViewModel>> GetArticlesWithActiveCategories()
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, BaseUrl, "?$expand=Category,CreatedBy,UpdatedBy,NewsTags($expand=Tag)");
            var response = await _httpClient.SendAsync(request);

            var result = await response.ReadContentAsync<ODataResponse<NewsArticleViewModel>>();

            return result.Value;
        }
    }
}
