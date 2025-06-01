using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NMS_API_FE.Services.Interfaces;

namespace NewsManagementSystem.Controllers
{
    public class GuestController : Controller
    {
        private readonly IGuestService _guestService;
        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        // GET: GuestController
        public ActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> All()
        {
            var list = await _guestService.GetArticlesWithActiveCategories();
            return View(list);
        }
    }
}
