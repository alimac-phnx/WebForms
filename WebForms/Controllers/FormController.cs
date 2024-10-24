using Microsoft.AspNetCore.Mvc;
using WebForms.Models;
using WebForms.ViewModels;
using WebForms.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebForms.Data;
using Microsoft.SqlServer.Server;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class FormController : Controller
{
    private readonly ApplicationDbContext _context;

    private readonly ITemplateRepository _templateService;

    public FormController(ApplicationDbContext context, ITemplateRepository templateService)
    {
        _context = context;
        _templateService = templateService;
    }

    public async Task<IActionResult> UserForms()
    {
        int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);

        var formViewModels = await (from form in _context.Forms
                           join template in _context.Templates
                           on form.TemplateId equals template.Id
                           where form.AssignedByUserId == userId
                           select new FormTableViewModel
                           {
                               Id = form.Id,
                               Name = template.Name,
                               Description = template.Description,
                               AssignedAt = form.AssignedAt,
                               AnswerCount = _context.Answers.Count(a => a.FormId == form.Id)
                           }).ToListAsync();

        return View(formViewModels);
    }

    // Метод для отображения страницы с формой на основе шаблона
    public async Task<IActionResult> Create(int id)
    {
        var template = await _templateService.GetByIdAsync(id);
        if (template == null)
        {
            return NotFound();
        }

        var topic = await _context.Topics.FindAsync(template.TopicId);
        var user = await _context.Users.FindAsync(template.CreatedByUserId);

        var viewModel = new FormCreateViewModel
        {
            Template = template,
            TopicName = topic.Name,
            QuestionsCount = template.Questions.Where(q => q.IsVisible).ToList().Count,
            AuthorName = user.Username
        };

        return View(viewModel);
    }

    // Метод для обработки данных после отправки формы
    [HttpPost]
    public async Task<IActionResult> Create(FormCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);

            var form = new Form()
            {
                TemplateId = model.Template.Id,
                AssignedByUserId = userId,
                AssignedAt = DateTime.Now,
                Answers = model.Answers
            };

            _context.Answers.AddRange(model.Answers);
            _context.Forms.Add(form);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        return View("Create", model); // Если валидация не прошла, отобразим форму заново
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var form = await _context.Forms.Include(f => f.Answers).FirstOrDefaultAsync(t => t.Id == id);
        if (form == null)
        {
            return NotFound();
        }

        var template = await _context.Templates.Include(f => f.Questions).FirstOrDefaultAsync(t => t.Id == form.TemplateId);
        var topic = await _context.Topics.FindAsync(template.TopicId);
        var user = await _context.Users.FindAsync(template.CreatedByUserId);

        // Создаем ViewModel для передачи данных в представление
        var model = new FormEditViewModel
        {
            FormId = form.Id,
            Template = template,
            TopicName = topic.Name,
            QuestionsCount = template.Questions.Where(q => q.IsVisible).ToList().Count,
            AuthorName = user.Username,
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
            var form = await _context.Forms.Include(f => f.Answers).FirstOrDefaultAsync(t => t.Id == model.FormId);
            if (form == null)
            {
                return NotFound();
            }

            form.Answers = model.Answers;
            form.AssignedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("UserForms");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var form = await _context.Forms.FindAsync(id);
        if (form == null)
        {
            return NotFound();
        }
        _context.Forms.Remove(form);
        _context.SaveChanges();
        return RedirectToAction("UserForms");
    }

    public async Task<IActionResult> Details(int id)
    {
        var form = await _context.Forms.Include(f => f.Answers).FirstOrDefaultAsync(t => t.Id == id);
        if (form == null)
        {
            return NotFound();
        }

        var template = await _context.Templates.Include(f => f.Questions).FirstOrDefaultAsync(t => t.Id == form.TemplateId);
        var topic = await _context.Topics.FindAsync(template.TopicId);
        var user = await _context.Users.FindAsync(template.CreatedByUserId);

        // Создаем ViewModel для передачи данных в представление
        var model = new FormEditViewModel
        {
            FormId = form.Id,
            Template = template,
            TopicName = topic.Name,
            QuestionsCount = template.Questions.Where(q => q.IsVisible).ToList().Count,
            AuthorName = user.Username,
            Answers = form.Answers
        };

        return View(model);
    }
}
