using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using BLL.Utils;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Staff")]
    public class UserController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        public UserController(INewsArticleService newsArticleService, IMapper mapper, IAccountService accountService)
        {
            _newsArticleService = newsArticleService;
            _mapper = mapper;
            _accountService = accountService;

        }
        [HttpGet("MyProfile/{userId}")]
        public async Task<ActionResult> MyProfile(int userId)
        {
            var user = _mapper.Map<AccountDTO>(await _accountService.GetAccountByIdAsync(userId));
            if (user == null)
            {
                return BadRequest("User not found");
            }
            return Ok(user);
        }

        [HttpPut("MyProfileUpdate/{userId}")]
        public async Task<ActionResult> MyProfile(int userId, AccountDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _accountService.UpdateAccountAsync(userId, dto);
                return Ok(dto);
            }
            return BadRequest(ModelState);
        }

        // GET: /Staff/MyNewsHistory
        [HttpGet("MyNewsHistory")]  
        public async Task<ActionResult> MyNewsHistory()
        {
            var userId = GetUserFromToken();
            var newsHistory = await _newsArticleService.GetNewsArticlesByUserIdAsync(userId);
            return Ok(newsHistory);
        }
            
        private int GetUserFromToken()
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            return int.Parse(userIdClaim.Value);
        }
    }
}
