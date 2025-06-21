using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace NewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;
        public GuestController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [HttpGet("Index")]
        public async Task<ActionResult> All()
        {
            var list = await _newsArticleService.GetArticlesWithActiveCategories();
            return Ok(list.AsQueryable());
        }
    }
}
