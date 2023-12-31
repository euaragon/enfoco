﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using enfoco2.Models;
using enfoco2.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using enfoco2.Data;

namespace enfoco2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly NoticeService _noticeService;
        private object _context;

        public HomeController(ILogger<HomeController> logger, NoticeService noticeService)
        {
            _logger = logger;
            _noticeService = noticeService;
        }


        public IActionResult Index(int page = 1, int pageSize = 6)
        {
            var allNotices = _noticeService.GetNotice().OrderByDescending(notice => notice.Id);

            var totalCount = allNotices.Count();
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var notices = allNotices.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new PagedResult<Notice>
            {
                Data = notices,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = pageCount
            };

            return View(model);
        }



        public IActionResult Editorial()
        {
            var editorialNotices = _noticeService.GetNotice().OrderByDescending(notice => notice.Id)
                .Where(notice => notice.Section == NoticeSection.category1)
                .ToList();

            return View(editorialNotices);
        }


        public IActionResult Noticias()
        {
            var noticiasNotices = _noticeService.GetNotice().OrderByDescending(notice => notice.Id)
                .Where(notice => notice.Section == NoticeSection.category2)
                .ToList();

            return View(noticiasNotices);
        }

        public IActionResult Analisis()
        {
            var analisisNotices = _noticeService.GetNotice().OrderByDescending(notice => notice.Id)
                .Where(notice => notice.Section == NoticeSection.category3)
                .ToList();

            return View(analisisNotices);
        }

       
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        private const string user = "enfoco";
        private const string password = "revista";




        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userEntered, string passwordEntered)
        {
            if (userEntered == user && passwordEntered == password)
            {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, userEntered), 
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index");
            }

          
            ModelState.AddModelError(string.Empty, "Credenciales no válidas");
            return View();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(int id)
        {

            var notice = await _noticeService.GetNoticeByIdAsync(id);

            if (notice == null)
            {
                return NotFound();
            }

            var noticeDto = new NoticeDto
            {
                Id = notice.Id,
                Title = notice.Title,
                Issue = notice.Issue,
                Subtitle = notice.Subtitle,
                Text = notice.Text,
                Img = notice.Img,
                IsFeatured = notice.IsFeatured,
                Category = notice.Category,
                Section = notice.Section

            };

            return View(noticeDto);
        }

       




        [HttpGet]
        public IActionResult Search(string searchTerm, int page = 1, int pageSize = 4)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                
                return RedirectToAction("Index");
            }

            var searchResults = _noticeService.SearchNotices(searchTerm);

            var totalCount = searchResults.Count();
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var notices = searchResults.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new PagedResult<Notice>
            {
                Data = notices,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = pageCount
            };


            return View("Index", model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Notice notice)
        {
            if (ModelState.IsValid)
            {
                await _noticeService.AddNoticeAsync(notice);
                return RedirectToAction("Index"); 
            }
            return View(notice);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var notice = await _noticeService.GetNoticeByIdAsync(id);
            if (notice == null)
            {
                return NotFound();
            }

            await _noticeService.DeleteNoticeAsync(id);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult SubscribeNewsletter(Newsletter newsletter, [FromServices] EnfocoDb dbContext)
        {
            if (ModelState.IsValid)
            {
                dbContext.Newsletter.Add(newsletter);
                dbContext.SaveChanges();
                return RedirectToAction("Suscribe");
            }
            return View("Index", newsletter);
        }

        
        public IActionResult Suscribe()
        {
            return View();
        }

        public IActionResult Suscriptores()
        {
            List<enfoco2.Models.Newsletter> listaDeSuscriptores = (List<Newsletter>)_noticeService.GetNewsletter(); 
            return View(listaDeSuscriptores);
        }

    }
}



