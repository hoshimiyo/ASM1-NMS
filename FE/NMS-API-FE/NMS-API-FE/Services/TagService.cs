
using Helpers;
using NMS_API_FE.DTOs;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;

namespace NMS_API_FE.Services
{
    public class TagService : ITagService
    {
        private readonly HttpClient _httpClient;
        public const string BaseUrl = "/api/Tag/"; // Adjust the base URL as needed

        public TagService(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task CreateTagAsync(TagDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl + "CreateTag", dto);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }

        public async Task DeleteTagAsync(int TagId)
        {
            var response = await _httpClient.DeleteAsync(BaseUrl + "DeleteTag/" + TagId);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }

        public async Task<IEnumerable<TagViewModel>> GetAllTagsAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "GetAllTags");
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
            var result = await response.ReadContentAsync<IEnumerable<TagViewModel>>();
            return result ?? Enumerable.Empty<TagViewModel>();
        }

        public async Task<TagViewModel> GetTagAsync(int TagId)
        {
            var response = await _httpClient.GetAsync(BaseUrl + "GetTag/" + TagId);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
            var result = await response.ReadContentAsync<TagViewModel>();
            return result ?? throw new KeyNotFoundException($"Tag with ID {TagId} not found.");
        }

        public async Task UpdateTagAsync(int TagId, TagDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync(BaseUrl + "UpdateTag/" + TagId, dto);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful, otherwise throw an exception
        }
    }
}
