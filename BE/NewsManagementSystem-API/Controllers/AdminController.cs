using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace NewsManagementSystem.Controllers
{
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        public AdminController(IAccountService accountService, IUnitOfWork unitOfWork)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;

        }

        // GET: /Admin/Report
        [HttpGet("Report")]
        public async Task<ActionResult> Report(DateTime startDate, DateTime endDate)
        {
            var reportData = await _unitOfWork.NewsArticles.GetAllByListAsync(n => n.CreatedDate >= startDate &&
                                                                                    n.CreatedDate <= endDate);
            return Ok(reportData.AsQueryable());

        }

        // GET: /Admin/SearchAccount
        [HttpGet("SearchAccount")]
        public async Task<ActionResult> SearchAccount(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest("Search term cannot be empty.");
            }

            var accounts = await _accountService.GetAllAccountsAsync();
            var filteredAccounts = accounts.Where(a => a.AccountName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                       a.AccountEmail.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!filteredAccounts.Any())
            {
                return Ok("No accounts found matching the search term.");
            }

            return Ok(filteredAccounts.AsQueryable());
        }
    }
}
