using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NMS_API_FE.ApiResponseModels;
using NMS_API_FE.DTOs;
using NMS_API_FE.Services.Interfaces;
using System.Text;
using System.Text.Json;

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

        public async Task<LoginApiResponse> Login(AccountLoginDTO dtos)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}Login", dtos);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Login failed: {errorContent}");
                }

                // Deserialize to simplified response
                return await response.Content.ReadFromJsonAsync<LoginApiResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                throw;
            }
        }

        public async Task Register(AccountCreateDTO dtos)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl + "Register", dtos);


            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }


        public async Task Logout()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/Logout");

            response.EnsureSuccessStatusCode();
        }
    }
}
