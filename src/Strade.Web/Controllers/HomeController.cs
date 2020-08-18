using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Strade.Web.Models;

namespace Strade.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index(LoginViewModel model)
        {
            if(!ModelState.IsValid) return View();
            return View();
        }

        
    }
}