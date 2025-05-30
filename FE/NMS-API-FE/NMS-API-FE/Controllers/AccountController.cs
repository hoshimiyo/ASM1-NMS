using Microsoft.AspNetCore.Mvc;
using NMS_API_FE.DTOs;
using NMS_API_FE.Services.Interfaces;
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

        public async Task<IActionResult> Login(AccountLoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                await _accountService.Login(loginDTO);
                return View(loginDTO);
            }
            return View("Index");
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
