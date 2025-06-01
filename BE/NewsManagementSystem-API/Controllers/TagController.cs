using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace NewsManagementSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost("CreateTag")]
        public async Task<ActionResult> CreateTagAsync(TagDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Tag data is null.");
            }
            await _tagService.CreateTagAsync(dto);
            return Ok("Tag created successfully.");
        }

        [HttpPut("UpdateTag/{tagId}")]
        public async Task<ActionResult> UpdateTagAsync(int TagId, TagDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Tag data is null.");
            }
            await _tagService.UpdateTagAsync(TagId, dto);
            return Ok("Tag updated successfully.");
        }

        [HttpDelete("DeleteTag/{tagId}")]
        public async Task<ActionResult> DeleteTagAsync(int TagId)
        {
            await _tagService.DeleteTagAsync(TagId);
            return Ok("Tag deleted successfully.");
        }

        [HttpGet("GetTag/{tagId}")]
        public async Task<ActionResult<Tag>> GetTagAsync(int TagId)
        {
            var tag = await _tagService.GetTagAsync(TagId);
            if (tag == null)
            {
                return NotFound("Tag not found.");
            }
            return Ok(tag);
        }

        [HttpGet("GetAllTags")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetAllTagsAsync()
        {
            var tags = await _tagService.GetAllTagsAsync();
            if (tags == null || !tags.Any())
            {
                return NotFound("No tags found.");
            }
            return Ok(tags);
        }
    }
}
