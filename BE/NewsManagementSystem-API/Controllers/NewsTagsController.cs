using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace NewsManagementSystem_API.Controllers
{
    [Authorize(Policy = "Staff")]
    public class NewsTagsController : ODataController
    {
        private readonly INewsTagService _newsTagService;

        public NewsTagsController(INewsTagService newsTagService)
        {
            _newsTagService = newsTagService;
        }

        // POST /odata/NewsTags
        public async Task<IActionResult> Post([FromBody] NewsTag newsTag)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _newsTagService.AddNewsTagAsync(newsTag.NewsArticleId, newsTag.TagId);
            return Created(newsTag);
        }

        // DELETE /odata/NewsTags(NewsArticleId='abc123',TagId=5)
        [HttpDelete]
        [Route("odata/NewsTags(NewsArticleId='{NewsArticleId}',TagId={TagId})")]
        public async Task<IActionResult> Delete([FromODataUri] string NewsArticleId, [FromODataUri] int TagId)

        {
            await _newsTagService.DeleteNewsTagAsync(NewsArticleId, TagId);
            return NoContent();
        }

    }
}
