using System.Security.Claims;

namespace NMS_API_FE.ApiResponseModels
{
    public class LoginApiResponse
    {
        public List<Claim> Token { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Role { get; set; }
    }
}
