using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebForms.Interfaces;
using WebForms.Models;
using WebForms.Services;
using WebForms.ViewModels;

namespace WebForms.Controllers
{
    [Authorize]
    public class TemplateController : Controller
    {
        private readonly TemplateService _templateService;

        public readonly IQuestionRepository _questionRepository;

        public readonly ITemplateRepository _templateRepository;

        public readonly ITopicRepository _topicRepository;
         
        public readonly ITagRepository _tagRepository;

        public readonly IUserRepository _userRepository;

        public TemplateController(TemplateService templateService, IQuestionRepository questionRepository, ITemplateRepository templateRepository, ITopicRepository topicRepository, ITagRepository tagRepository, IUserRepository userRepository)
        {
            _templateService = templateService;
            _questionRepository = questionRepository;
            _templateRepository = templateRepository;
            _topicRepository = topicRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> UserTemplates()
        {
            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
            var templates = await _templateService.GetUserTemplates(userId);

            return View(templates);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tags = await _templateService.GetAllTags();

            var model = new TemplateCreateViewModel
            {
                AvailableTopics = await _templateService.GetAllTopics(),
                AvailableTags = tags.Select(t => t.Name).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TemplateCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);

                await _templateService.CreateTemplate(model, userId);

                return RedirectToAction("UserTemplates");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var template = await _templateService.GetTemplate(id);

            if (template == null) { return NotFound(); }

            var tags = await _templateService.GetAllTags();

            var editTemplate = new TemplateEditViewModel()
            {
                TemplateId = template.Id,
                Name = template.Name,
                Description = template.Description,
                TopicId = template.TopicId,
                AvailableTopics = await _templateService.GetAllTopics(),
                ImageUrl = template.ImageUrl,
                CurrentTags = template.Tags.Select(t => t.Name).ToList(),
                AvailableTags = tags.Select(t => t.Name).ToList(),
                Questions = template.Questions
            };

            return View(editTemplate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TemplateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var template = await _templateService.GetTemplate(model.TemplateId);

                if (model != null)
                {
                    template.Name = model.Name;
                    template.Description = model.Description;
                    template.TopicId = model.TopicId;
                    template.Tags = new List<Tag>();

                    if (Request.Form["RemoveImage"] == "true") { template.ImageUrl = null; }
                    else { await _templateService.AddImageToTemplate(model.NewImageFile, template); }

                    await _templateService.UpdateTemplate(model.NewTags, model.Questions, template);

                    return RedirectToAction("UserTemplates");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var template = await _templateRepository.GetByIdAsync(id);

            if (template == null)
            {
                return NotFound();
            }

            await _templateRepository.DeleteAsync(id);

            return RedirectToAction("UserTemplates");
        }


        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var template = await _templateRepository.GetByIdAsync(id);

            if (template == null)
            {
                return NotFound();
            }

            var topic = await _topicRepository.GetByIdAsync(template.TopicId);
            var user = await _userRepository.GetByIdAsync(template.CreatedByUserId);

            var viewModel = new FormCreateViewModel
            {
                Template = template,
                TopicName = topic.Name,
                QuestionsCount = template.Questions.Where(q => q.IsVisible).ToList().Count,
                AuthorName = user.Username
            };

            return View(viewModel);
        }

        [AllowAnonymous]
        public IActionResult RedirectToFormOrTemplateView(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Create", "Form", new { id });
            }
            else
            {
                return RedirectToAction("Details", new { id });
            }
        }
    }
}
