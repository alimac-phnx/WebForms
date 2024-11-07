using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForms.Models;
using System.Security.Claims;
using WebForms.ViewModels;
using WebForms.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

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
                SetAuthenticationAsync(await _accountService.GetUserAsync(email));

                SetCookie(email);
                
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Incorrect e-mail or password");
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
        public void SetAuthenticationAsync(User user)
        {
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(GetClaimsIdentity(user)), GetAuthProperties());
        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
            var user = await _accountService.GetUserByIdAsync(userId);

            var viewmodel = new RegistrationDataViewModel
            {
                Username = user.Username,
                Email = user.Email,
            };

            return View(viewmodel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSalesforceAccount()
        {
            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "00DQy00000EpNiU!AQEAQKb_Q2QfFczhh9qDYqjn6Fap47UU_CqnSP32YTXCAGhZLdR8KKQ.G9lDpF_agoDdSpWHmJfAsZkVDZoNGGF3ZnLkXpIj");

            var accountContent = await _accountService.PrepareSfAccount(userId);

            var response = await client.PostAsync("https://takeorg-dev-ed.develop.my.salesforce.com/services/data/v56.0/sobjects/Account", accountContent);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var contactContent = await _accountService.PrepareSfAccountContact(result, accountContent);

                await client.PostAsync("https://takeorg-dev-ed.develop.my.salesforce.com/services/data/v56.0/sobjects/Contact", contactContent);
            }
            else ModelState.AddModelError("", "Unexpected error");

            return RedirectToAction("MyProfile");
        }
    }
}