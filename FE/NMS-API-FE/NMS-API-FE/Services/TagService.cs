
using Helpers;
using NMS_API_FE.DTOs;
using NMS_API_FE.Helpers;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class TagService : ITagService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/odata/Tags"; // Adjust the base URL as needed
        private readonly IHttpContextAccessor _contextAccessor;

        public TagService(HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(_contextAccessor));
        }

        public async Task CreateTagAsync(TagDTO dto)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Post, BaseUrl, "", dto);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }

        public async Task DeleteTagAsync(int TagId)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Delete, BaseUrl, $"({TagId})");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }

        public async Task<IEnumerable<TagViewModel>> GetAllTagsAsync()
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, BaseUrl, $"");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
            var result = await response.ReadContentAsync<ODataResponse<TagViewModel>>();
            return result.Value ?? Enumerable.Empty<TagViewModel>();
        }

        public async Task<TagViewModel> GetTagAsync(int TagId)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Get, BaseUrl, $"({TagId})");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
            var result = await response.ReadContentAsync<TagViewModel>();
            return result ?? throw new KeyNotFoundException($"Tag with ID {TagId} not found.");
        }

        public async Task UpdateTagAsync(int TagId, TagDTO dto)
        {
            var request = await HttpClientExtensions.GenerateRequest(_contextAccessor, HttpMethod.Put, BaseUrl, $"({TagId})", dto);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }
    }
}
