using NMS_API_FE.DTOs;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/api/Admin/"; // Adjust the base URL as needed

        public AdminService(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<SystemAccountViewModel>> ManageAccounts()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "ManageAccounts");

            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<SystemAccountViewModel>>();

            if (result == null)
            {
                throw new InvalidOperationException("The response content is null or could not be deserialized into SystemAccountViewModel.");
            }

            return result;
        }
        public async Task<SystemAccountViewModel> DetailsAccount(int id)
        {
            var response = await _httpClient.GetAsync(BaseUrl + "DetailsAccount/" + id);

            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception

            var account = await response.Content.ReadFromJsonAsync<SystemAccountViewModel>();

            if (account == null)
            {
                throw new InvalidOperationException("The response content is null or could not be deserialized into SystemAccountViewModel.");
            }

            return account;
        }

        public async Task CreateAccount(AccountCreateAdminDTO accountCreateAdminDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl + "CreateAccount", accountCreateAdminDTO);

            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }

        public async Task EditAccount(int id, AccountUpdateAdminDTO accountUpdateAdminDTO)
        {
            var response = await _httpClient.PutAsJsonAsync(BaseUrl + "EditAccount/" + id, accountUpdateAdminDTO);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }

        public async Task DeleteAccount(int id)
        {
            var response = await _httpClient.DeleteAsync(BaseUrl + "DeleteAccount/" + id);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }

        public async Task<List<NewsArticleViewModel>> Report(DateTime startDate, DateTime endDate)
        {

            var response = await _httpClient.GetAsync($"{BaseUrl}Report?startDate={startDate}&endDate={endDate}");

            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception

            var result = await response.Content.ReadFromJsonAsync<List<NewsArticleViewModel>>();

            if (result == null)
            {
                throw new InvalidOperationException("The response content is null or could not be deserialized into NewsArticleViewModel.");
            }

            return result;
        }
        public async Task<IEnumerable<SystemAccountViewModel>> SearchAccount(string searchTerm)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}SearchAccount?searchTerm={searchTerm}");

            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<SystemAccountViewModel>>();

            if (result == null)
            {
                throw new InvalidOperationException("The response content is null or could not be deserialized into SystemAccountViewModel.");
            }

            return result;
        }
    }
}
