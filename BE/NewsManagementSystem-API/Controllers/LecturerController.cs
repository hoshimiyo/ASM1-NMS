using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

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
            var list = await _newsArticleService.GetArticlesWithActiveCategories();
            return Ok(list.AsQueryable());
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
