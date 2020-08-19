using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Strade.Data.Entities;
using Strade.Web.Models;
using Strade.Web.Services;

namespace Strade.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountsService _accountsService;

        public AccountsController(
           SignInManager<ApplicationUser> signInManager,
           IAccountsService accountsService)
        {
            _signInManager = signInManager;
            _accountsService = accountsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if(!ModelState.IsValid) return View();
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.MatricNo, model.Password, false, false);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Login failed, please check your details");
                    return View();
                }
                return LocalRedirect("~/Dashboard");
            }
            catch (Exception e)
            {
     
                ModelState.AddModelError("", e.Message);
                return View();
            }
 
 
        }

        
    }
}