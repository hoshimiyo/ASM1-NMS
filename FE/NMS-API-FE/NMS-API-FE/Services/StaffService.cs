using NMS_API_FE.DTOs;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class StaffService : IStaffService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/api/Staff/"; // Adjust the base URL as needed

        public StaffService(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<CategoryService>> GetActiveCategories()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "GetActiveCategories");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<CategoryService>>();
            return result ?? Enumerable.Empty<CategoryService>();
        }

        public async Task CreateCategory(CategoryDTO categoryViewModel)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl + "CreateCategory", categoryViewModel);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCategory(int id, CategoryViewModel categoryViewModel)
        {
            var response = await _httpClient.PutAsJsonAsync(BaseUrl + "UpdateCategory/" + id, categoryViewModel);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategory(int id)
        {
            var response = await _httpClient.DeleteAsync(BaseUrl + "DeleteCategory?id=" + id);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<NewsArticleViewModel>> GetNewsArticles()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "ManageNewsArticle");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<NewsArticleViewModel>>();
            return result ?? Enumerable.Empty<NewsArticleViewModel>();
        }

        public async Task<NewsArticleViewModel> GetNewsArticleById(string id)
        {
            var response = await _httpClient.GetAsync(BaseUrl + "NewsArticleDetail/" + id);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<NewsArticleViewModel>();
            return result ?? throw new InvalidOperationException("Article not found");
        }

        public async Task CreateNewsArticle(NewsArticleCreateDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl + "CreateNewsArticle", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateNewsArticle(string id, NewsArticleUpdateDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync(BaseUrl + "EditNewsArticle/" + id, dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteNewsArticle(string id)
        {
            var response = await _httpClient.DeleteAsync(BaseUrl + "DeleteNewsArticle/" + id);
            response.EnsureSuccessStatusCode();
        }

        public async Task<AccountDTO> GetMyProfile()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "MyProfile");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<AccountDTO>();
            return result ?? new AccountDTO(); // Return an empty AccountDTO if null
        }

        public async Task<IEnumerable<IEnumerable<NewsArticleViewModel>>> GetNewsHistory()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "MyNewsHistory");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<IEnumerable<NewsArticleViewModel>>>();
            return result ?? Enumerable.Empty<IEnumerable<NewsArticleViewModel>>(); // Return an empty collection if null
        }

        public async Task UpdateMyProfile(AccountDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync(BaseUrl + "MyProfileUpdate", dto);
            response.EnsureSuccessStatusCode();
        }
    }
}
