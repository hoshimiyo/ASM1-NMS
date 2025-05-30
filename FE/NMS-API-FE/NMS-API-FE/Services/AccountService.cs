using Microsoft.AspNetCore.Mvc;
using NMS_API_FE.DTOs;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/api/Account/"; // Adjust the base URL as needed

        public AccountService(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<HttpResponseMessage> Login(AccountLoginDTO dtos)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/Login", dtos);

            return response;
        }

        public async Task Logout()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/Logout");
            
            response.EnsureSuccessStatusCode();
        }

        public async Task Register(AccountCreateDTO dtos)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/Register", dtos);

            response.EnsureSuccessStatusCode();
        }
    }
}
