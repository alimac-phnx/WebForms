using Microsoft.AspNetCore.Mvc;
using WebForms.Models;
namespace WebForms.Controllers
{
    public class HomeController : Controller
    {
        private readonly TemplateService _templateService;

        public HomeController(TemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var templates = await _templateService.GetAllTemplatesAsync();

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
    }
}