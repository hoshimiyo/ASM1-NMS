using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace NewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INewsArticleService _newsArticleService;
        private readonly IMapper _mapper;
        public AdminController(IAccountService accountService, IUnitOfWork unitOfWork, INewsArticleService newsArticleService, IMapper mapper)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _newsArticleService = newsArticleService;
            _mapper = mapper;
        }

        // GET: /Admin/ManageAccounts
        [HttpGet("ManageAccounts")]
        public async Task<ActionResult> ManageAccounts()
        {
            var accounts = await _accountService.GetAllAccountsForManageAsync();
            return Ok(accounts);
        }

        [HttpPost("DetailsAccount/{id}")]
        public async Task<ActionResult<SystemAccount>> DetailsAccount(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();
            return account;
        }


        // POST: /Admin/CreateAccount
        [HttpPost("CreateAccount")]
        public async Task<ActionResult> CreateAccount(AccountCreateAdminDTO account)
        {
            if (ModelState.IsValid)
            {
                await _accountService.CreateAccountAsync(account);
                return Ok(account);
            }
            return StatusCode(400, ModelState);
        }

        // POST: /Admin/EditAccount/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> EditAccount(int id, AccountUpdateAdminDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _accountService.UpdateAccountAsync(id, dto);
                return Ok(dto);
            }
            return StatusCode(400, ModelState);
        }



        // POST: /Admin/DeleteAccount/{id}
        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();

            await _accountService.DeleteAccountAsync(id);
            return Ok();
        }

        // GET: /Admin/Report
        [HttpGet("Report")]
        public async Task<ActionResult> Report(DateTime startDate, DateTime endDate)
        {
            var reportData = await _unitOfWork.NewsArticles.GetAllByListAsync(n => n.CreatedDate >= startDate &&
                                                                                    n.CreatedDate <= endDate);
            return Ok(reportData);

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

            return Ok(filteredAccounts);
        }
    }
}
