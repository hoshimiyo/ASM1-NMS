using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace NewsManagementSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsTagRelationsController : ControllerBase
    {
        private readonly INewsTagService _newsTagService;

        public NewsTagRelationsController(INewsTagService service)
        {
            _newsTagService = service;
        }

        [HttpGet("GetTagsOfArticleAsync/{NewsArticleId}")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTagsOfArticleAsync(string NewsArticleId)
        {
            if (string.IsNullOrEmpty(NewsArticleId))
            {
                return BadRequest("Invalid NewsArticleId.");
            }
            var tags = await _newsTagService.GetTagsOfArticleAsync(NewsArticleId);
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
    }

}
