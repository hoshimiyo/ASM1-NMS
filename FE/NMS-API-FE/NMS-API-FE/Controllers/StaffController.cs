using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS_API_FE.DTOs;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.Controllers
{
    public class StaffController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IStaffService _staffService;
        private readonly ICategoryService _categoryService;
        private readonly INewsArticlesService _newsArticleService;
        public StaffController (IMapper mapper, IStaffService staffService, ICategoryService categoryService, INewsArticlesService newsArticleService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _staffService = staffService ?? throw new ArgumentNullException(nameof(staffService));
            _categoryService = categoryService;
            _newsArticleService = newsArticleService;
        }

        // GET: /Staff/ManageCategories
        public async Task<IActionResult> ManageCategories()
        {
            var categories = await _staffService.GetActiveCategories();
            return View(categories);
        }

        // GET: /Staff/CreateCategory
        public IActionResult CreateCategory()
        {
            return View();
        }

        // POST: /Staff/CreateCategory
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _staffService.CreateCategory(dto);
                return RedirectToAction(nameof(ManageCategories));
            }
            return View(dto);
        }

        // GET: /Staff/EditCategory/{id}
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: /Staff/EditCategory/{id}
        [HttpPost]
        public async Task<IActionResult> EditCategory(int id, CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryAsync(id, category);
                return RedirectToAction(nameof(ManageCategories));
            }
            return View(category);
        }

        // POST: /Staff/DeleteCategory/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return RedirectToAction(nameof(ManageCategories));
        }

        // GET: /Staff/ManageNewsArticles
        public async Task<IActionResult> ManageNewsArticles()
        {
            var articles = await _staffService.GetNewsArticles();
            return View(articles);
        }

        // GET: /Staff/CreateNewsArticle
        public IActionResult CreateNewsArticle()
        {
            return View();
        }

        // POST: /Staff/CreateNewsArticle
        [HttpPost]
        public async Task<IActionResult> CreateNewsArticle(NewsArticleCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _staffService.CreateNewsArticle(dto);
                return RedirectToAction(nameof(ManageNewsArticles));
            }
            return View(dto);
        }

        // GET: /Staff/EditNewsArticle/{id}
        public async Task<IActionResult> EditNewsArticle(string id)
        {
            var article = await _newsArticleService.GetNewsArticleByIdAsync(id);
            if (article == null) return NotFound();
            return View(article);
        }

        // POST: /Staff/EditNewsArticle/{id}
        [HttpPost]
        public async Task<IActionResult> EditNewsArticle(string id, NewsArticleUpdateDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _newsArticleService.UpdateNewsArticleAsync(id, dto);
                return RedirectToAction(nameof(ManageNewsArticles));
            }
            return View(dto);
        }

        // POST: /Staff/DeleteNewsArticle/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteNewsArticle(string id)
        {
            await _newsArticleService.DeleteNewsArticleAsync(id);
            return RedirectToAction(nameof(ManageNewsArticles));
        }

        // GET: /Staff/MyProfile
        public async Task<IActionResult> MyProfile()
        {
            var user = await _staffService.GetMyProfile();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> MyProfile(AccountDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _staffService.UpdateMyProfile(dto);
                TempData["SuccessMessage"] = "Your profile has been updated successfully!";
                return RedirectToAction(nameof(MyProfile));
            }
            return View(dto);
        }

        // GET: /Staff/MyNewsHistory
        public async Task<IActionResult> MyNewsHistory()
        {
            var userId = GetUserFromToken();
            var newsHistory = await _staffService.GetNewsHistory();
            return View(newsHistory);
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
