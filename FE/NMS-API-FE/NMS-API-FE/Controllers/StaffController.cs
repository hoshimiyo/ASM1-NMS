using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS_API_FE.DTOs;
using NMS_API_FE.Models;
using NMS_API_FE.Services.Interfaces;
using NMS_API_FE.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.Controllers
{
    public class StaffController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IStaffService _staffService;
        private readonly ICategoryService _categoryService;
        private readonly INewsArticlesService _newsArticleService;
        public StaffController (IMapper mapper, IStaffService staffService, ICategoryService categoryService, INewsArticlesService newsArticleService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _staffService = staffService ?? throw new ArgumentNullException(nameof(staffService));
            _categoryService = categoryService;
            _newsArticleService = newsArticleService;
        }

        // GET: /Staff/MyProfile
        public async Task<IActionResult> MyProfile()
        {
            var token = Request.Cookies["JwtToken"];
            var userIdClaim = JwtUtils.GetClaimValue(token, JwtRegisteredClaimNames.Sub);
            var user = await _staffService.GetMyProfile(int.Parse(userIdClaim));
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> MyProfile(AccountDTO dto)
        {

            if (ModelState.IsValid)
            {
                var token = Request.Cookies["JwtToken"];
                var userIdClaim = JwtUtils.GetClaimValue(token, JwtRegisteredClaimNames.Sub);
                await _staffService.UpdateMyProfile(int.Parse(userIdClaim), dto);
                TempData["SuccessMessage"] = "Your profile has been updated successfully!";
                return RedirectToAction(nameof(MyProfile));
            }
            return View(dto);
        }

        // GET: /Staff/MyNewsHistory
        public async Task<IActionResult> MyNewsHistory()
        {
            var userId = GetUserFromToken();
            var newsHistory = await _staffService.GetNewsHistory();
            return View(newsHistory);
        }

        private int GetUserFromToken()
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            return int.Parse(userIdClaim.Value);
        }
    }
}
