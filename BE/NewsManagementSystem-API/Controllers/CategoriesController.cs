using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;

namespace NewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "Staff")]
    public class CategoriesController : ControllerBase
    {
        private readonly NewsContext _context;

        public CategoriesController(NewsContext context)
        {
            _context = context;
        }

        // GET: Categories
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult> GetAllCategories()
        {
            var newsContext = _context.Categories.Include(c => c.ParentCategory);
            return Ok(newsContext);
        }

        // GET: Categories/Details/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST: Categories/Create
        [HttpPost("Create")]
        public async Task<ActionResult> Create([Bind("CategoryId,CategoryName,CategoryDescription,IsActive,ParentCategoryId")] Category category) 
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(category);
            //    await _context.SaveChangesAsync();
            //    return Ok(category);
            //}

            ////"ParentCategoryId" = new SelectList(_context.Categories, "CategoryId", "CategoryDescription", category.ParentCategoryId);

            //return BadRequest(category);
            throw new NotImplementedException();
        }


        // POST: Categories/Edit/5
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,CategoryDescription,IsActive,ParentCategoryId")] Category category)
        {
            //if (id != category.CategoryId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(category);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!CategoryExists(category.CategoryId))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return Ok(category);
            //}
            //return BadRequest(category);
            throw new NotImplementedException();
        }


        // POST: Categories/Delete/5
        [HttpDelete("DeleteConfirmed")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete category.");
            }
            return Ok("Deletion completed");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
