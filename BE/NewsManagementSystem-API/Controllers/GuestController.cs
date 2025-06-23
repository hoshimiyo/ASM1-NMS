using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace NewsManagementSystem.Controllers
{
    public class GuestController : ODataController
    {
        private readonly INewsArticleService _newsArticleService;
        public GuestController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [EnableQuery]
        public async Task<ActionResult> Get()
        {
            var list = await _newsArticleService.GetArticlesWithActiveCategories();
            return Ok(list.AsQueryable());
        }
    }
}
