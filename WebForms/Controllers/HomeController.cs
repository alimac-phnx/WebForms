using Microsoft.AspNetCore.Mvc;
using WebForms.Models;
using WebForms.Services.Interfaces;
namespace WebForms.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITemplateService _templateService;

        public HomeController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var templates = (await _templateService.GetAllTemplatesAsync());

            var viewModels = _templateService.PrepareTemplatesToDisplay(templates);

            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<Template>());
            }

            var templates = await _templateService.SearchTemplatesByQueryAsync(query);

            var viewModels = _templateService.PrepareTemplatesToDisplay(templates);

            return View("Index", viewModels);
        }

        [HttpPost]
        public IActionResult SetTheme(string theme)
        {
            if (!string.IsNullOrEmpty(theme))
            {
                var options = new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddYears(1),
                    IsEssential = true
                };
                Response.Cookies.Append("theme", theme, options);
            }
            return Redirect(Request.Headers.Referer.ToString());
        }
    }
}