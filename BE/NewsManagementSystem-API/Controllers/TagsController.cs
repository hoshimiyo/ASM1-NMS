using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Formatter;

namespace NewsManagementSystem_API.Controllers
{
    [Authorize(Policy = "Staff")]
    public class TagsController : ODataController
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: odata/Tag
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var tags = await _tagService.GetAllTagsAsync();
            if (tags == null || !tags.Any())
                return NotFound("No tags found.");

            return Ok(tags.AsQueryable());
        }

        // GET: odata/Tag(1)
        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var tag = await _tagService.GetTagAsync(key);
            if (tag == null)
                return NotFound("Tag not found.");

            return Ok(tag);
        }

        // POST: odata/Tag
        public async Task<IActionResult> Post([FromBody] TagDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
                return BadRequest(ModelState);

            await _tagService.CreateTagAsync(dto);
            return Created(dto);
        }

        // PUT: odata/Tag(1)
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] TagDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
                return BadRequest(ModelState);

            await _tagService.UpdateTagAsync(key, dto);
            return Updated(dto);
        }

        // DELETE: odata/Tag(1)
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            await _tagService.DeleteTagAsync(key);
            return NoContent(); // 204
        }
    }
}
