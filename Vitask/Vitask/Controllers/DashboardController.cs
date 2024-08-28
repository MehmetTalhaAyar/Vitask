using Microsoft.AspNetCore.Mvc;

namespace Vitask.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
