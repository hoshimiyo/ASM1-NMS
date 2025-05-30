using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "Staff")]
    public class StaffController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsArticleService _newsArticleService;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        public StaffController(ICategoryService categoryService, INewsArticleService newsArticleService, IMapper mapper, IAccountService accountService)
        {
            _categoryService = categoryService;
            _newsArticleService = newsArticleService;
            _mapper = mapper;
            _accountService = accountService;

        }

        // GET: /Staff/ManageCategories
        [HttpGet("ManageCategories")]
        public async Task<ActionResult> ManageCategories()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(categories);
        }

        // POST: /Staff/CreateCategory
        [HttpPost("CreateCategory")]
        public async Task<ActionResult> CreateCategory(CategoryDTO dto)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(dto);
                await _categoryService.CreateCategoryAsync(category);
                return RedirectToAction(nameof(ManageCategories));
            }
            return Ok(dto);
        }

        // POST: /Staff/EditCategory/{id}
        [HttpPut("EditCategory/{id}")]
        public async Task<ActionResult> EditCategory(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryAsync(id, category);
                return Ok(category);
            }
            return BadRequest(ModelState);
        }

        // POST: /Staff/DeleteCategory/{id}
        [HttpDelete("DeleteCategory")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeactiveCategoryAsync(id);
                return Ok("Category deleted successfully."); // Only return success if no exception
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState); // Return 400 Bad Request with error details
            }
        }

        // GET: /Staff/ManageNewsArticles
        [HttpGet("ManageNewsArticles")]
        public async Task<ActionResult> ManageNewsArticles()
        {
            var articles = await _newsArticleService.GetActiveNewsArticlesAsync();
            return Ok(articles);
        }

        // POST: /Staff/CreateNewsArticle
        [HttpPost("CreateNewsArticle")]
        public async Task<ActionResult> CreateNewsArticle(NewsArticleCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _newsArticleService.CreateNewsArticleAsync(dto, HttpContext);
                return Ok(dto);
            }
            return BadRequest(ModelState);
        }

        // GET: /Staff/EditNewsArticle/{id}
        [HttpGet("EditNewsArticle/{id}")]
        public async Task<ActionResult> EditNewsArticle(string id)
        {
            var article = await _newsArticleService.GetNewsArticleByIdAsync(id);
            if (article == null) return NotFound();
            return Ok(article);
        }

        // POST: /Staff/EditNewsArticle/{id}
        [HttpPut("EditNewsArticle/{id}")]
        public async Task<ActionResult> EditNewsArticle(string id, NewsArticleUpdateDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _newsArticleService.UpdateNewsArticleAsync(id, dto, HttpContext);
                return Ok(dto);
            }
            return BadRequest(ModelState);
        }

        // POST: /Staff/DeleteNewsArticle/{id}
        [HttpDelete("DeleteNewsArticle/{id}")]
        public async Task<ActionResult> DeleteNewsArticle(string id)
        {
            await _newsArticleService.DeactiveNewsArticleAsync(id);
            return Ok("News article deleted successfully."); // Only return success if no exception
        }

        // GET: /Staff/MyProfile'
        [HttpGet("MyProfile")]
        public async Task<ActionResult> MyProfile()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim);
            var user = _mapper.Map<AccountDTO>(await _accountService.GetAccountByIdAsync(userId));
            return Ok(user);
        }

        [HttpPut("MyProfileUpdate")]
        public async Task<ActionResult> MyProfile(AccountDTO dto)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var userId = int.Parse(userIdClaim);
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
