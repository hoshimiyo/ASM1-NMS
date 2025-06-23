using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Security.Claims;

namespace NewsManagementSystem.Controllers
{
    public class NewsArticlesController : ODataController
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsArticlesController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var articles = await _newsArticleService.GetArticlesWithActiveCategories();
            return Ok(articles.AsQueryable());
        }

        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] string key)
        {
            var article = await _newsArticleService.GetNewsArticleByIdAsync(key);
            if (article == null)
                return NotFound();

            return Ok(article);
        }

        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> Post([FromBody] NewsArticleCreateDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var article = await _newsArticleService.CreateNewsArticleAsync(dto, userId);
            return Created(article);
        }

        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> Put([FromODataUri] string key, [FromBody] NewsArticleUpdateDTO dto, [FromODataUri] int userId)
        {
            await _newsArticleService.UpdateNewsArticleAsync(key, dto, userId);
            return Updated(dto);
        }

        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> Delete([FromODataUri] string key)
        {
            await _newsArticleService.DeactiveNewsArticleAsync(key);
            return NoContent();
        }
    }
}
