using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForms.Models;
using WebForms.Services;
using WebForms.ViewModels;

namespace WebForms.Controllers
{
    [Authorize]
    public class TemplateController : Controller
    {
        private readonly TemplateService _templateService;

        public readonly ImageService _imageService;

        public readonly TopicService _topicService;
         
        public readonly TagService _tagService;

        public TemplateController(TemplateService templateService, TopicService topicService, TagService tagService, ImageService imageService)
        {
            _templateService = templateService;
            _imageService = imageService;
            _topicService = topicService;
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> UserTemplates()
        {
            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
            var templates = await _templateService.GetUserTemplatesAsync(userId);

            return View(templates);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tags = await _tagService.GetAllTagsAsync();

            var model = new TemplateCreateViewModel
            {
                AvailableTopics = await _topicService.GetAllTopicsAsync(),
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

                await _templateService.CreateTemplateAsync(model, userId);

                return RedirectToAction("UserTemplates");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var template = await _templateService.GetTemplateByIdAsync(id);

            if (template == null) return NotFound();

            var tags = await _tagService.GetAllTagsAsync();

            var editTemplate = new TemplateEditViewModel()
            {
                TemplateId = template.Id,
                Name = template.Name,
                Description = template.Description,
                TopicId = template.TopicId,
                AvailableTopics = await _topicService.GetAllTopicsAsync(),
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
                var template = await _templateService.GetTemplateByIdAsync(model.TemplateId);

                if (model != null)
                {
                    template.Name = model.Name;
                    template.Description = model.Description;
                    template.TopicId = model.TopicId;
                    template.Tags = new List<Tag>();

                    if (Request.Form["RemoveImage"] == "true") { template.ImageUrl = null; }
                    else { await _imageService.AddImageToTemplateAsync(model.NewImageFile, template); }

                    await _templateService.UpdateTemplateAsync(model.Tags, model.Questions, template);

                    return RedirectToAction("UserTemplates");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var template = await _templateService.GetTemplateByIdAsync(id);

            if (template == null) return NotFound();

            await _templateService.DeleteTemplateAsync(id);

            return RedirectToAction("UserTemplates");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var template = await _templateService.GetTemplateByIdAsync(id);

            if (template == null) return NotFound();

            var topic = await _topicService.GetTopicByIdAsync(template.TopicId);

            var viewModel = new FormCreateViewModel
            {
                Template = template,
                TopicName = topic.Name,
                QuestionsCount = template.Questions.Where(q => q.IsVisible).ToList().Count,
                AuthorName = await _templateService.GetTemplateAuthorNameAsync(template.CreatedByUserId)
            };

            return View(viewModel);
        }

        [AllowAnonymous]
        public IActionResult RedirectToFormOrTemplateView(int id)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Create", "Form", new { id });
            else return RedirectToAction("Details", new { id });
        }
    }
}