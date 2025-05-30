using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "Lecturer")]
    public class LecturerController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;
        public LecturerController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [HttpGet("GetArticlesWithActiveCategories")]
        public async Task<ActionResult> GetArticlesWithActiveCategories()
        {
            return Ok(await _newsArticleService.GetArticlesWithActiveCategories());
        }

        // GET: LecturerController/Details/5
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
    }
}
