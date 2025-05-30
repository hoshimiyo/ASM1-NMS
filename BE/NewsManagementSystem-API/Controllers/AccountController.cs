using BLL.DTOs;
using BLL.Interfaces;
using BLL.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsManagementSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
        private readonly UserUtils _userUtils;
        public AccountController(IAccountService accountService, UserUtils userUtils, IConfiguration configuration)
        {
            _accountService = accountService;
            _userUtils = userUtils;
            _configuration = configuration;
        }

        // POST: /Account/Login
        [HttpPost("Login")]
        public async Task<ActionResult<ClaimsIdentity>> Login(string email, string password)
        {
            string adminEmail = _configuration["AdminCredentials:Email"];
            string adminPassword = _configuration["AdminCredentials:Password"];

            if (email == adminEmail && password == adminPassword)
            {
                var adminClaims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, "Admin"),
                    new(ClaimTypes.Name, "Administrator"),
                    new(ClaimTypes.Role, "3")
                };

                var adminIdentity = new ClaimsIdentity(adminClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var adminPrincipal = new ClaimsPrincipal(adminIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, adminPrincipal);
                HttpContext.User = adminPrincipal;

                return Ok(new
                {
                    Token = adminClaims,
                    UserId = "Admin",
                    Name = "Administrator",
                    Role = "3"
                });
            }


            var account = await _accountService.AuthenticateAsync(email, password);
            if (account == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Define cookie options for the new token
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IsPersistent = true
            };

            //// Retrieve user account details after authentication
            //var user = await _accountService.GetAccountByIdAsync(_userUtils.GetUserFromInputToken(token));

            var claims = new List<Claim>
                {
                new(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new(ClaimTypes.Name, account.AccountName),
                new(ClaimTypes.Role, account.AccountRole.ToString())
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            HttpContext.User = principal;


            var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Token = claims,
                UserId = account.AccountId,
                Name = account.AccountName,
                Role = account.AccountRole
            });
        }

        // POST: /Admin/CreateAccount
        [HttpPost("Register")]
        public async Task<IActionResult> Register(AccountCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _accountService.CreateAccountAsync(dto);
                return Ok();
            }
            return StatusCode(400, ModelState);
        }

        // GET: /Account/Logout
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out successfully." });
        }
    }
}
