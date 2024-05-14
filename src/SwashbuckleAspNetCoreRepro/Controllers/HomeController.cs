using System.Diagnostics;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SwashbuckleAspNetCoreRepro.Models;

namespace SwashbuckleAspNetCoreRepro.Controllers;

/// <summary>
/// <para>[ApiVersionNeutral] means that aspnet-api-versioning won't consider controllers derived from this
/// class to be API controllers. Therefore, we don't have to set a default API version in Startup.cs.</para>
/// <para>See: https://github.com/Microsoft/aspnet-api-versioning/issues/80#issuecomment-284731607 </para>
/// <para>[ApiExplorerSettings] means that we want to exclude these controllers and their children from Swagger.</para>
/// </summary>
[ApiVersionNeutral]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="logger"></param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Index
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Privacy
    /// </summary>
    /// <returns></returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Error
    /// </summary>
    /// <returns></returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
