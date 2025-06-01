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
using BLL.Interfaces;
using BLL.DTOs;

namespace NewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "Staff")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Categories
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult> GetAllCategories()
        {
            var newsContext = await _categoryService.GetActiveCategoriesAsync();
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

            var category = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST: Categories/Create
        [HttpPost("Create")]
        public async Task<ActionResult> Create(Category categoryCreateDTO)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategoryAsync(categoryCreateDTO);
                return Ok(categoryCreateDTO);
            }
            return BadRequest(categoryCreateDTO);
        }


        // POST: Categories/Edit/5
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Category categoryCreateDTO)
        {
            if (id != categoryCreateDTO.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.UpdateCategoryAsync(id, categoryCreateDTO);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(categoryCreateDTO.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(categoryCreateDTO);
            }
            return BadRequest(categoryCreateDTO);
        }


        // POST: Categories/Delete/5
        [HttpDelete("DeleteConfirmed")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category != null)
            {
                try
                {
                    await _categoryService.DeactiveCategoryAsync(id);
                }
                catch (DbUpdateException)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete category.");
                }
            }


            return Ok("Deletion completed");
        }

        private bool CategoryExists(int id)
        {
            return _categoryService.GetCategoryByIdAsync(id) != null;
        }
    }
}
