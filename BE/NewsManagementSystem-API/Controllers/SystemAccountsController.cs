using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace NewsManagementSystem.Controllers
{
    [Authorize(Policy = "Admin")]
    public class SystemAccountsController : ODataController
    {
        private readonly IAccountService _accountService;

        public SystemAccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var accounts = await _accountService.GetAllAccountsForManageAsync();
            return Ok(accounts.AsQueryable());
        }

        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var account = await _accountService.GetAccountByIdAsync(key);
            if (account == null) return NotFound();
            return Ok(account);
        }

        public async Task<IActionResult> Post([FromBody] AccountCreateAdminDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _accountService.CreateAccountAsync(dto);
            return Created(result);
        }

        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] AccountUpdateAdminDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _accountService.UpdateAccountAsync(key, dto);
            return Updated(dto);
        }

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var account = await _accountService.GetAccountByIdAsync(key);
            if (account == null) return NotFound();

            var result = await _accountService.DeleteAccountAsync(key);

            if (!result.Success)
                return (BadRequest(result.Message));

            return Ok(result.Message);
        }
    }
}
