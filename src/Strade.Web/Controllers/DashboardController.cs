using Microsoft.AspNetCore.Mvc;

namespace Strade.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}