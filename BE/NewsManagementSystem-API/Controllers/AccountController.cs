using BLL.DTOs;
using BLL.Interfaces;
using BLL.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

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
        private readonly JwtUtils _jwtUtils;
        public AccountController(IAccountService accountService, UserUtils userUtils, IConfiguration configuration, JwtUtils jwtUtils)
        {
            _accountService = accountService;
            _userUtils = userUtils;
            _configuration = configuration;
            _jwtUtils = jwtUtils;
        }

        // POST: /Account/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login(AccountLoginDTO dtos)
        {
            string adminEmail = _configuration["AdminCredentials:Email"];
            string adminPassword = _configuration["AdminCredentials:Password"];
            string token;
            if (dtos.accountEmail == adminEmail && dtos.accountPassword == adminPassword)
            {
                token = _jwtUtils.GenerateAccessTokenAdmin();
                Console.WriteLine("ADMIN TOKEN :"+ token);
                return Ok(new { token });
            }

            token = await _accountService.AuthenticateAsync(dtos);

            if (token == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            return Ok(new { token });

        }

        // POST: /Admin/Register
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
