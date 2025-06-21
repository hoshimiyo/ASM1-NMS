using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using NMS_API_FE.Services.Interfaces;
using NMS_API_FE.Models;
using System.Data;

namespace NewsManagementSystem.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var newsContext = await _categoryService.GetAllCategoriesAsync();
            return View(newsContext);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public async Task <IActionResult> Create()
        {
            // Fetch all categories for the dropdown
            var allCategories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.ParentCategoryId = new SelectList(allCategories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,CategoryDescription,IsActive,ParentCategoryId")] CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Fetch all categories for the dropdown
            var allCategories = await _categoryService.GetAllCategoriesAsync();

            // Exclude the current category from being selected as its own parent (to avoid circular reference)
            var parentOptions = allCategories.Where(c => c.CategoryId != category.CategoryId).ToList();

            ViewBag.ParentCategoryId = new SelectList(parentOptions, "CategoryId", "CategoryName", category.ParentCategoryId);

            return View(category);
        }


        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,CategoryDescription,IsActive,ParentCategoryId")] CategoryViewModel category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.UpdateCategoryAsync(id, category);
                }
                catch (DBConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdown on validation failure
            var allCategories = await _categoryService.GetAllCategoriesAsync();
            var parentOptions = allCategories.Where(c => c.CategoryId != category.CategoryId).ToList();
            ViewBag.ParentCategoryId = new SelectList(parentOptions, "CategoryId", "CategoryName", category.ParentCategoryId);

            return View(category);
        }


        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            //var hasValidCategory = await _context.NewsArticles.AnyAsync(n => n.CategoryId == category.CategoryId);

            //if (hasValidCategory)
            //{
            //    TempData["Error"] = "Category has Articles";
            //    return RedirectToAction(nameof(Index), new { id = id });
            //}

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = _categoryService.GetCategoryByIdAsync(id);

            if (category != null)
            {
                await _categoryService.DeleteCategoryAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _categoryService.GetCategoryByIdAsync(id).Result != null;
        }
    }
}
