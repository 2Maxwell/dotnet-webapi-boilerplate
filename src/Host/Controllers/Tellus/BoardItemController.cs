using Microsoft.AspNetCore.Mvc;

namespace FSH.WebApi.Host.Controllers.Tellus;
public class BoardItemController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
