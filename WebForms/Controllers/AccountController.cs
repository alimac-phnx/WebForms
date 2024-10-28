using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForms.Models;
using System.Security.Claims;
using WebForms.ViewModels;
using WebForms.Services.Interfaces;

namespace WebForms.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return RedirectAuthUser();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegistrationDataViewModel registrationData)
        {
            if (ModelState.IsValid && await _accountService.TryRegisterAsync(registrationData))
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid) { ModelState.AddModelError("", "The e-mail is already in use"); }
            return View(registrationData);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return RedirectAuthUser();
        }

        public IActionResult RedirectAuthUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (await _accountService.LoginAsync(email, password))
            {
                await SetAuthenticationAsync(await _accountService.GetUserAsync(email));

                SetCookie(email);
                
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Incorrect username or password");
            return View();
        }

        public void SetCookie(string email)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true,
                IsEssential = true
            };

            HttpContext.Response.Cookies.Append("UserId", _accountService.GetUserAsync(email).Result.Id.ToString(), options);
        }

        public ClaimsIdentity GetClaimsIdentity(User user)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };

            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public AuthenticationProperties GetAuthProperties()
        {
            return new AuthenticationProperties { IsPersistent = true };
        }

        public async Task<RedirectToActionResult> SetAuthenticationAsync(User user)
        {
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(GetClaimsIdentity(user)), GetAuthProperties());

            return RedirectToAction("UserManager", "Account");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
}