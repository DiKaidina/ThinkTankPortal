using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using ThinkThank.Portal.Models;

namespace ThinkThank.Portal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _env;
    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public IActionResult Index(string culture)
    {
        GetCulture(culture);
        return View();
    }

    public IActionResult About()
    {
        return View();
    }
    
    public IActionResult NotFound(int? statusCode)
    {
        return View(statusCode is 404 ? "_NotFound" : "_Error");
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile data, UploadFormRequest request)
    {
        var path = Path.Combine(_env.WebRootPath + "\\downloads", data.FileName);
        await using (var stream = new FileStream(path, FileMode.Create))
        {
             await data.CopyToAsync(stream);
        }

        return View("Index");
    }

    public IActionResult Contact()
    {
        return View();
    }
    public string GetCulture(string code = "")
    {
        if (string.IsNullOrWhiteSpace(code)) return "";
        CultureInfo.CurrentCulture = new CultureInfo(code);
        CultureInfo.CurrentUICulture = new CultureInfo(code);
        ViewBag.Culture =
            $"CurrentCulture: {CultureInfo.CurrentCulture}, CurrentUICulture: {CultureInfo.CurrentUICulture}";
        return "";
    }
    
}