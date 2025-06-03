using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace NMS_API_FE.Utils
{
    public static class JwtUtils
    {
        public static ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var identity = new ClaimsIdentity(jwtToken.Claims);
            return new ClaimsPrincipal(identity);
        }
            
        public static string GetClaimValue(string token, string claimType)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }

    }
}
