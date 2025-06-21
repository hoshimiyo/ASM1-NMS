using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NMS_API_FE.ApiResponseModels;
using NMS_API_FE.DTOs;
using NMS_API_FE.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace NMS_API_FE.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        public const string BaseUrl = "/api/Account/"; // Adjust the base URL as needed

        public AccountService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
            _contextAccessor = httpContextAccessor;
        }

        public async Task<LoginApiResponse> Login(AccountLoginDTO dtos)
        {
            try
            {
                var token = _contextAccessor.HttpContext.Request.Cookies["JwtToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Post, BaseUrl, "Login", dtos);
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return null;
                }

                // Deserialize to simplified response
                return await response.ReadContentAsync<LoginApiResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                throw;
            }
        }

        public async Task Register(AccountCreateDTO dtos)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Post, BaseUrl, "Register", dtos);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }


        public async Task Logout()
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, BaseUrl, "Logout");
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }
    }
}
