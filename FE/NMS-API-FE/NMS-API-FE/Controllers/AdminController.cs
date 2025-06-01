using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS_API_FE.DTOs;
using NMS_API_FE.Services.Interfaces;

namespace NewsManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;
        public AdminController(IAdminService adminService, IMapper mapper)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        // GET: /Admin/ManageAccounts
        public async Task<IActionResult> ManageAccounts()
        {
            var result = await _adminService.ManageAccounts();
            return View(result);
        }

        public async Task<IActionResult> DetailsAccount(int id)
        {

            var account = await _adminService.DetailsAccount(id);
            if (account == null) return NotFound();
            return View(account);
        }


        // GET: /Admin/CreateAccount
        public IActionResult CreateAccount()
        {
            return View();
        }

        // POST: /Admin/CreateAccount
        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountCreateAdminDTO account)
        {
            if (ModelState.IsValid)
            {
                await _adminService.CreateAccount(account);
                TempData["Message"] = "Account created successfully.";
                return RedirectToAction(nameof(ManageAccounts));
            }
            TempData["Error"] = "Failed to create account.";
            return View(account);
        }

        // GET: /Admin/EditAccount/{id}
        [HttpGet]
        public async Task<IActionResult> EditAccount(int id)
        {
            var account = await _adminService.DetailsAccount(id);
            if (account == null) return NotFound();
            var accountMapped = (_mapper.Map<AccountUpdateAdminDTO>(account));
            return View(accountMapped);
        }

        // POST: /Admin/EditAccount/{id}
        [HttpPost]
        public async Task<IActionResult> EditAccount(int id, AccountUpdateAdminDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _adminService.EditAccount(id, dto);
                TempData["Message"] = "Account updated successfully.";
                return RedirectToAction(nameof(ManageAccounts));
            }
            TempData["Error"] = "Failed to update account.";
            return View(dto);
        }
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _adminService.DetailsAccount(id);
            if (account == null) return NotFound();
            return View(account);
        }



        // POST: /Admin/DeleteAccount/{id}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _adminService.DeleteAccount(id);
            TempData["Message"] = "Account deleted successfully.";
            return RedirectToAction(nameof(ManageAccounts));
        }

        // GET: /Admin/Report
        public async Task<IActionResult> Report(DateTime startDate, DateTime endDate)
        {
            var reportData = await _adminService.Report(startDate, endDate);
            return View(reportData);

        }

        // POST: /Admin/GenerateReport
        [HttpPost]
        public async Task<IActionResult> GenerateReport(DateTime startDate, DateTime endDate)
        {
            var reportData = await _adminService.Report(startDate, endDate);
            return View(reportData);
        }

        // GET: /Admin/SearchAccount
        public async Task<IActionResult> SearchAccount(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                TempData["Error"] = "Search term cannot be empty.";
                return RedirectToAction(nameof(ManageAccounts));
            }

            var accounts = await _adminService.SearchAccount(searchTerm);

            return View("ManageAccounts", accounts);
        }
    }
}
