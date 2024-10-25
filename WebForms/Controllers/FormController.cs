using Microsoft.AspNetCore.Mvc;
using WebForms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebForms.Services;

[Authorize]
public class FormController : Controller
{
    private readonly FormService _formService;

    private readonly TemplateService _templateService;

    private readonly TopicService _topicService;

    private readonly QuestionService _questionService;

    public FormController(FormService formService, TemplateService templateService, TopicService topicService, QuestionService questionService)
    {
        _formService = formService;
        _templateService = templateService;
        _topicService = topicService;
        _questionService = questionService;
    }

    [HttpGet]
    public async Task<IActionResult> UserForms()
    {
        int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);

        var forms = await _formService.GetFormsInfoAsync(userId);

        var viewModels = forms.Select(form => new FormTableViewModel
        {
            Id = form.Id,
            Name = form.Name,
            Description = form.Description,
            AssignedAt = form.AssignedAt,
            AnswerCount = form.AnswerCount
        }).ToList();

        return View(viewModels);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int id)
    {
        var template = await _templateService.GetTemplateByIdAsync(id);

        if (template == null) return NotFound();

        var topic = await _topicService.GetTopicByIdAsync(template.TopicId);

        var viewModel = new FormCreateViewModel
        {
            Template = template,
            TopicName = topic.Name,
            QuestionsCount = template.Questions.Where(q => q.IsVisible).ToList().Count,
            AuthorName = await _templateService.GetTemplateAuthorNameAsync(template.CreatedByUserId),
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(FormCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);

            await _formService.CreateForm(model.Answers, model.Template.Id, userId);

            return RedirectToAction("Index", "Home");
        }

        return View("Create", model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var form = await _formService.GetFormByIdAsync(id);

        if (form == null) return NotFound();

        var template = await _templateService.GetTemplateByIdAsync(form.TemplateId);

        var model = new FormEditViewModel
        {
            FormId = form.Id,
            Template = template,
            TopicName = await _topicService.GetTopicNameAsync(template.TopicId),
            QuestionsCount = (await _questionService.GetVisibleQuestions(form.TemplateId)).Count,
            AuthorName = await _templateService.GetTemplateAuthorNameAsync(template.CreatedByUserId),
            AssignedAt = form.AssignedAt,
            Answers = form.Answers
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(FormEditViewModel model)
    {
        if (ModelState.IsValid)
        {
            var form = await _formService.GetFormByIdAsync(model.FormId);

            if (form == null) return NotFound();

            await _formService.UpdateFormAsync(model.Answers, form);

            return RedirectToAction("UserForms");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var form = await _formService.GetFormByIdAsync(id);

        if (form == null) return NotFound();

        await _formService.DeleteFormAsync(id);

        return RedirectToAction("UserForms");
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var form = await _formService.GetFormByIdAsync(id);

        if (form == null) return NotFound();

        var template = await _templateService.GetTemplateByIdAsync(form.TemplateId);

        var model = new FormEditViewModel
        {
            FormId = form.Id,
            Template = template,
            TopicName = await _topicService.GetTopicNameAsync(template.TopicId),
            QuestionsCount = (await _questionService.GetVisibleQuestions(form.TemplateId)).Count,
            AssignedAt = form.AssignedAt,
            AuthorName = await _templateService.GetTemplateAuthorNameAsync(form.AssignedByUserId),
            Answers = form.Answers
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> FormsByTemplate(int id)
    {
        var template = await _templateService.GetTemplateByIdAsync(id);

        var forms = await _formService.GetAllByTemplateAsync(id);

        var viewModels = forms.Select(form => new FormEditViewModel
        {
            FormId = form.Id,
            Template = template,
            QuestionsCount = template.Questions.Where(q => q.IsVisible).ToList().Count,
            TopicName = _topicService.GetTopicNameAsync(template.TopicId).Result,
            AssignedAt = form.AssignedAt,
            AuthorName = _templateService.GetTemplateAuthorNameAsync(form.AssignedByUserId).Result
        }).ToList();

        return View(viewModels);
    }
}