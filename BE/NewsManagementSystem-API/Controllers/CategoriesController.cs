using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace NewsManagementSystem.Controllers
{
    [Authorize(Policy = "Staff")]
    public class CategoriesController : ODataController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: odata/Categories
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(categories.AsQueryable());
        }

        // GET: odata/Categories(1)
        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var category = await _categoryService.GetCategoryByIdAsync(key);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // POST: odata/Categories
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _categoryService.CreateCategoryAsync(category);
            return Created(category);
        }

        // PUT: odata/Categories(1)
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Category category)
        {
            if (key != category.CategoryId)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _categoryService.UpdateCategoryAsync(key, category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update: {ex.Message}");
            }

            return Updated(category);
        }

        // DELETE: odata/Categories(1)
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var category = await _categoryService.GetCategoryByIdAsync(key);
            if (category == null)
                return NotFound();

            try
            {
                await _categoryService.DeactiveCategoryAsync(key);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete category.");
            }

            return NoContent();
        }
    }
}
