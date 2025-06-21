using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NMS_API_FE.DTOs;
using NMS_API_FE.Services.Interfaces;
using NMS_API_FE.Utils;
using System.Data;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace NMS_API_FE.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                // Return the same view with validation errors
                return View(model);
            }

            // Your login logic
            var result = await _accountService.Login(model);

            if (result.Token == null)
            {
                TempData["Error"] = "Invalid email or password.";
                return RedirectToAction("Login", "Account");
            }
            var role = int.Parse(JwtUtils.GetClaimValue(result.Token, "role"));

            var existingToken = Request.Cookies["JwtToken"];
            if (existingToken != null)
            {
                Response.Cookies.Delete("JwtToken");
            }

            Response.Cookies.Append("JwtToken", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            // Redirect based on the user's role
            return role switch
            {
                3 => RedirectToAction("ManageAccounts", "Admin"),
                1 => RedirectToAction("Index", "Categories"),
                2 => RedirectToAction("All", "Lecturer"),
                _ => RedirectToAction("All", "Guest") // Default fallback
            };
        }
        

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountCreateDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                await _accountService.Register(registerDTO);
                return RedirectToAction(nameof(Login));
            }
            return View(ModelState);
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("JwtToken");
            return RedirectToAction("All", "Guest");
        }


    }
}
