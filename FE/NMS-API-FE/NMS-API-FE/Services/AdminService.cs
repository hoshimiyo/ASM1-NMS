using Helpers;
using NMS_API_FE.DTOs;
using NMS_API_FE.Helpers;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;
using System.Net.Http.Headers;

namespace NMS_API_FE.Services
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        public const string BaseUrl = "/odata/SystemAccounts"; // Adjust the base URL as needed
        public const string AdminUrl = "/api/Admin/";

        public AdminService(HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
        }

        public async Task<IEnumerable<SystemAccountViewModel>> ManageAccounts()
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, BaseUrl, "");

            var response = await _httpClient.SendAsync(request);


            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception

            var result = await response.ReadContentAsync<ODataResponse<SystemAccountViewModel>>();

            if (result == null)
            {
                throw new InvalidOperationException("The response content is null or could not be deserialized into SystemAccountViewModel.");
            }

            return result.Value;
        }
        public async Task<SystemAccountViewModel> DetailsAccount(int id)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, BaseUrl, $"({id})");
            var response = await _httpClient.SendAsync(request);

            var account = await response.ReadContentAsync<SystemAccountViewModel>();

            if (account == null)
            {
                throw new InvalidOperationException("The response content is null or could not be deserialized into SystemAccountViewModel.");
            }

            return account;
        }

        public async Task CreateAccount(AccountCreateAdminDTO accountCreateAdminDTO)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Post, BaseUrl, "", accountCreateAdminDTO);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }

        public async Task EditAccount(int id, AccountUpdateAdminDTO accountUpdateAdminDTO)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Put, BaseUrl, $"({id})", accountUpdateAdminDTO);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }

        public async Task<bool> DeleteAccount(int id)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Delete, BaseUrl, $"({id})");
            var response = await _httpClient.SendAsync(request);

            if(response.IsSuccessStatusCode.Equals(false))
            {
                return false;
            }
            else return true;
        }

        public async Task<List<NewsArticleViewModel>> Report(DateTime startDate, DateTime endDate)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, AdminUrl, $"Report?startDate={startDate}&endDate={endDate}");
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception

            var result = await response.ReadContentAsync<JsonArrayWrapper<NewsArticleViewModel>>();

            if (result == null)
            {
                throw new InvalidOperationException("The response content is null or could not be deserialized into NewsArticleViewModel.");
            }

            return result.Values;
        }
        public async Task<IEnumerable<SystemAccountViewModel>> SearchAccount(string searchTerm)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, AdminUrl, $"SearchAccount?searchTerm={searchTerm}");
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception

            var result = await response.ReadContentAsync<IEnumerable<SystemAccountViewModel>>();

            if (result == null)
            {
                throw new InvalidOperationException("The response content is null or could not be deserialized into SystemAccountViewModel.");
            }

            return result;
        }
    }
}
