using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
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

        // --------------------------------------------------
        // REGISTER
        // --------------------------------------------------

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
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

        // --------------------------------------------------
        // LOGIN
        // --------------------------------------------------

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

            // A successful login must have a valid user.
            if (result.User == null)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to complete login. User information was not found.");

                return View(vm);
            }

            var user = result.User;

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("UserName", user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(
                claims,
                "TradeSimCookie");

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                "TradeSimCookie",
                principal);

            return RedirectToAction(
                "Dashboard",
                "Dashboard");
        }

        // --------------------------------------------------
        // LOGOUT
        // --------------------------------------------------

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("TradeSimCookie");

            return RedirectToAction("Login");
        }
    }
}