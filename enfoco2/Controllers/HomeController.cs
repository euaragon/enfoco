using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using enfoco2.Models;
using enfoco2.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Linq;

namespace enfoco2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly NoticeService _noticeService;

        public HomeController(ILogger<HomeController> logger, NoticeService noticeService)
        {
            _logger = logger;
            _noticeService = noticeService;
        }


        public IActionResult Index(int page = 1, int pageSize = 4)
        {
            var allNotices = _noticeService.GetNotice();

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


        public IActionResult Fiscalia()
        {
            return View();
        }
        public IActionResult Etica()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        private const string user = "revista-enfoco";
        private const string password = "r3v1st4";




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
                    new Claim(ClaimTypes.Name, userEntered), // Puedes agregar otros reclamos según tus necesidades
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index");
            }

            // Si las credenciales no son válidas, mostrar un mensaje de error o redirigir a la página de inicio de sesión nuevamente.
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
                Img = notice.Img
            };

            return View(noticeDto);
        }


        [HttpGet]
        public IActionResult Search(string searchTerm, int page = 1, int pageSize = 4)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                // Puedes manejar la búsqueda vacía aquí si lo deseas.
                // En este ejemplo, simplemente redirigimos al método Index sin hacer nada.
                return RedirectToAction("Index");
            }

            // Realiza la búsqueda en la base de datos utilizando el servicio NoticeService.
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

            // Renderiza la vista Index con los resultados de búsqueda.
            return View("Index", model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Notice notice)
        {
            if (ModelState.IsValid)
            {
                // Guarda la noticia en la base de datos
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
    }
}



