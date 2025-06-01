
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NMS_API_FE.Services.Interfaces;

namespace NewsManagementSystem.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ILecturerService _lecturerService;
        public LecturerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }

        // GET: LecturerController
        public ActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> All()
        {
            return View(await _lecturerService.GetNewsArticles());
        }

        // GET: LecturerController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _lecturerService.GetNewsArticleById(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

    }
}
