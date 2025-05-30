using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Entities;
using BLL.Interfaces;
using BLL.DTOs;
using System.Security.Claims;
using BLL.Utils;

namespace NewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly INewsTagService _newsTagService;
        private readonly ITagService _tagService;

        public NewsArticlesController(INewsArticleService newsArticleService, ICategoryService categoryService, INewsTagService newsTagService, ITagService tagService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _newsTagService = newsTagService;
            _tagService = tagService;
        }

        // GET: NewsArticles
        [HttpGet("GetAllArticles")]
        public async Task<ActionResult> GetAllArticles()
        {
            var list = await _newsArticleService.GetArticlesWithActiveCategories();
            return Ok(list);

        }


        // GET: NewsArticles/Details/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            return Ok(newsArticle);
        }

        // POST: NewsArticles/Create
        [HttpPost("Create")]
        public async Task<ActionResult> Create(NewsArticleCreateDTO newsArticle)
        {
            if (ModelState.IsValid)
            {
                await _newsArticleService.CreateNewsArticleAsync(newsArticle, HttpContext);
                return Ok(new { message = "Article created successfully." });
            }

            return BadRequest(new { message = "Failed to create article.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        // POST: NewsArticles/Edit/5
        [HttpPut("Edit/{id}")]
        public async Task<ActionResult> Edit(string id, NewsArticleUpdateDTO newsArticle)
        {
            if (ModelState.IsValid)
            {
                await _newsArticleService.UpdateNewsArticleAsync(id, newsArticle, HttpContext);
                return Ok(new { message = "Article updated successfully." });
            }
            return BadRequest(new { message = "Failed to update article.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        // POST: NewsArticles/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            await _newsArticleService.DeactiveNewsArticleAsync(id);
            return Ok(new { message = "Article deleted successfully." });
        }

        // GET: NewsArticles/Search
        [HttpGet("Search")]
        public async Task<ActionResult> Search(string searchTerm, int? categoryId, int? tagId)
        {
            var articles = await _newsArticleService.GetActiveNewsArticlesAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                articles = articles.Where(a => a.NewsTitle.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                               a.Headline.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (categoryId.HasValue)
            {
                articles = articles.Where(a => a.CategoryId == categoryId.Value).ToList();
            }

            if (tagId.HasValue)
            {
                try
                {
                    var taggedArticles = await _newsTagService.GetArticlesFromTagAsync(tagId.Value);
                    articles = articles.Intersect(taggedArticles).ToList();

                    // If no articles match the tag filter, return an empty list
                    if (!articles.Any())    
                    {
                        return Ok(articles);
                    }
                }
                catch(KeyNotFoundException)
                {
                    return Ok(articles); // If tag not found, return the articles without filtering by tag
                }
            }

            return Ok(articles);
        }
    }
}
