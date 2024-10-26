using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForms.Services;
using WebForms.Models;
using System.Security.Claims;
using WebForms.Data;

namespace WebForms.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return RedirectAuthUser();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegistrationData registrationData)
        {
            if (ModelState.IsValid && await _accountService.TryRegisterAsync(registrationData))
            {
                return View("Login");
            }

            if (ModelState.IsValid) { ModelState.AddModelError("", "The e-mail is already in use"); }

            return View(registrationData);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return RedirectAuthUser();
        }

        [NonAction]
        public IActionResult RedirectAuthUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Index", "Home");
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
                
                return View("Index", "Home");
            }

            ModelState.AddModelError("", "Incorrect username or password");

            return View();
        }

        [NonAction]
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

        [NonAction]
        public ClaimsIdentity GetClaimsIdentity(User user)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };

            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [NonAction]
        public AuthenticationProperties GetAuthProperties()
        {
            return new AuthenticationProperties { IsPersistent = true };
        }

        [NonAction]
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

            return View("Login", "Account");
        }
    }
}