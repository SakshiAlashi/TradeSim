using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TradeSim.Models.Results;
using TradeSim.Models.ViewModels;
using TradeSim.Services;

namespace TradeSim.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService userService;

        public AccountController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        // The RegisterViewModel object is created and populated
        // automatically by ASP.NET Core Model Binding.
        public IActionResult Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            RegistrationResult result = userService.Register(vm);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(vm);
            }

            return RedirectToAction("Register");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            LoginResult result = userService.Login(vm);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new Claim("UserId", result.User.Id.ToString()),
                new Claim(ClaimTypes.Name, result.User.FullName),
                new Claim("UserName", result.User.UserName),
                new Claim(ClaimTypes.Email, result.User.Email)
            };

            var identity = new ClaimsIdentity(claims,"TradeSimCookie");

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("TradeSimCookie", principal);
            return RedirectToAction("Dashboard", "Dashboard");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("TradeSimCookie");

            return RedirectToAction("Login");
        }
    }
}
