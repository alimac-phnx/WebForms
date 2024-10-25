using Microsoft.AspNetCore.Mvc;
using WebForms.Models;
using WebForms.Services;
using WebForms.ViewModels;


namespace WebForms.Controllers
{
    public class HomeController : Controller
    {
        private readonly TemplateService _templateService;

        private readonly TopicService _topicService;

        public HomeController(TemplateService templateService, TopicService topicService)
        {
            _templateService = templateService;
            _topicService  = topicService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var templates = await _templateService.GetAllTemplatesAsync();

            var userIds = templates.Select(t => t.CreatedByUserId).Distinct().ToList();
            
            var viewModel = templates.Select(template => new TemplateDisplayViewModel
            {
                Template = template,
                QuestionsCount = template.Questions.Where(q => q.IsVisible).ToList().Count,
                TopicName = _topicService.GetTopicNameAsync(template.TopicId).Result,
                AuthorName = _templateService.GetTemplateAuthorNameAsync(template.CreatedByUserId).Result
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<Template>());
            }

            var searchResults = _templateService.SearchTemplateAuthorNameAsync(query);

            return View(searchResults);
        }
    }
}