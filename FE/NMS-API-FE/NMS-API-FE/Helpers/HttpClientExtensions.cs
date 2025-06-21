using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Helpers
{
    public static class HttpClientExtensions
    {
        public static async Task<T> ReadContentAsync<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == false)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Console.WriteLine("RESPONSE BODY: " + dataAsString);

            var result = JsonSerializer.Deserialize<T>(
                dataAsString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }

        public static async Task<HttpRequestMessage> GenerateRequest(IHttpContextAccessor contextAccessor, HttpMethod method, string baseUrl, string relativePath, object? dto = null)
        {
            var token = contextAccessor.HttpContext.Request.Cookies["JwtToken"];
            Console.WriteLine("Retrieved token from cookie: " + token);

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("No Token Present");
            }

            var request= new HttpRequestMessage(method, baseUrl + relativePath);

            if (dto != null && (method == HttpMethod.Post || method == HttpMethod.Put || method.Method == "PATCH"))
            {
                var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return request;
        }
    }
}
