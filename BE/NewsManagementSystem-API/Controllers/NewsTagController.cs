using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace NewsManagementSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsTagController : Controller
    {
        private readonly INewsTagService _newsTagService;
        public NewsTagController(INewsTagService newsTagService)
        {
            _newsTagService = newsTagService;
        }

        [HttpPost("AddNewsTag/{NewsArticleId}/{TagId}")]
        public async Task<ActionResult> AddNewsTagAsync(string NewsArticleId, int TagId)
        {
            if (string.IsNullOrEmpty(NewsArticleId) || TagId <= 0)
            {
                return BadRequest("Invalid NewsArticleId or TagId.");
            }
            await _newsTagService.AddNewsTagAsync(NewsArticleId, TagId);
            return Ok("News tag added successfully.");
        }

        [HttpGet("GetTagsOfArticleAsync/{NewsArticleId}")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTagsOfArticleAsync(string NewsArticleId)
        {
            if (string.IsNullOrEmpty(NewsArticleId))
            {
                return BadRequest("Invalid NewsArticleId.");
            }

            var tags = await _newsTagService.GetTagsOfArticleAsync(NewsArticleId);

            if (tags == null)
            {
                return NotFound($"Article with ID {NewsArticleId} not found");
            }

            // Empty collection is a valid response (HTTP 200)
            return Ok(tags);
        }

        [HttpGet("GetArticlesFromTag/{TagId}")]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> GetArticlesFromTagAsync(int TagId)
        {
            if (TagId <= 0)
            {
                return BadRequest("Invalid TagId.");
            }
            var articles = await _newsTagService.GetArticlesFromTagAsync(TagId);
            return Ok(articles);
        }

        [HttpDelete("DeleteNewsTag/{NewsArticleId}/{TagId}")]
        public async Task<ActionResult> DeleteNewsTagAsync(string NewsArticleId, int TagId)
        {
            if (string.IsNullOrEmpty(NewsArticleId) || TagId <= 0)
            {
                return BadRequest("Invalid NewsArticleId or TagId.");
            }
            await _newsTagService.DeleteNewsTagAsync(NewsArticleId, TagId);
            return Ok("News tag deleted successfully.");
        }
    }
}
