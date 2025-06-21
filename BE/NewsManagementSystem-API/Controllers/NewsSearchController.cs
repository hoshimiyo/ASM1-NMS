using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewsManagementSystem_API.Controllers
{
    [Authorize(Policy = "Staff")]
    [Route("api/[controller]")]
    [ApiController]
    public class NewsSearchController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly INewsTagService _newsTagService;

        public NewsSearchController(INewsArticleService articleService, INewsTagService tagService)
        {
            _newsArticleService = articleService;
            _newsTagService = tagService;
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string searchTerm, int? categoryId, int? tagId)
        {
            var articles = await _newsArticleService.GetActiveNewsArticlesAsync();

            if (!string.IsNullOrEmpty(searchTerm))
                articles = articles.Where(a => a.NewsTitle.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                               a.Headline.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            if (categoryId.HasValue)
                articles = articles.Where(a => a.CategoryId == categoryId).ToList();

            if (tagId.HasValue)
            {
                try
                {
                    var tagged = await _newsTagService.GetArticlesFromTagAsync(tagId.Value);
                    articles = articles.Intersect(tagged).ToList();
                }
                catch (KeyNotFoundException) { /* Skip tag filter */ }
            }

            return Ok(articles.AsQueryable());
        }
    }

}
