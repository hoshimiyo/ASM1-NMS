using Helpers;
using NMS_API_FE.DTOs;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;
using System.Net.Http.Headers;

namespace NMS_API_FE.Services
{
    public class StaffService : IStaffService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        public const string BaseUrl = "/api/Staff/"; // Adjust the base URL as needed
        public const string UserUrl = "/api/User/";

        public StaffService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
            _contextAccessor = httpContextAccessor;
        }

        public async Task<AccountDTO> GetMyProfile(int userId)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, UserUrl, $"MyProfile/{userId}");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<AccountDTO>();
            return result ?? new AccountDTO(); // Return an empty AccountDTO if null
        }

        public async Task<IEnumerable<IEnumerable<NewsArticleViewModel>>> GetNewsHistory()
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, UserUrl, $"MyNewsHistory");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.ReadContentAsync<IEnumerable<IEnumerable<NewsArticleViewModel>>>();
            return result ?? Enumerable.Empty<IEnumerable<NewsArticleViewModel>>(); // Return an empty collection if null
        }

        public async Task UpdateMyProfile(int userId, AccountDTO dto)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Put, UserUrl, $"MyProfileUpdate/{userId}", dto);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
