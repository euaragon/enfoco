using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using enfoco2.Models;
using enfoco2.Services;

namespace enfoco2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly NoticeService _noticeService;

    public HomeController(ILogger<HomeController> logger, NoticeService noticeService)
    {
        _logger = logger;
        _noticeService = noticeService;
    }


    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Investigacion()
    {
        return View();
    }
    public IActionResult Agenda()
    {
        return View();
    }

    public async Task<IActionResult> Detail(int id)
    {
        var notice = await _noticeService.GetNoticeByIdAsync(id);

        if (notice == null)
        {
            return NotFound();
        }

        return View(notice);
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

