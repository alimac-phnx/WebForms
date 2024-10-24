using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebForms.Data;
using WebForms.Models;
using System.Linq;
using WebForms.ViewModels;


namespace WebForms.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ������� ��������
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // �������� ����� ���������� ������� �� ���������� ����������� ����
            var popularTemplates = await _context.Templates
                .Include(t => t.Questions)
                .Include(t => t.Tags)
                .OrderByDescending(t => t.Questions.Count)
                .ToListAsync();

            // �������� �������������� �������������, ��������� �������
            var userIds = popularTemplates.Select(t => t.CreatedByUserId).Distinct().ToList();

            // ��������� �������������
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();
            var topics = await _context.Topics.ToListAsync();
            // ������� ������ �������������
            var viewModel = popularTemplates.Select(template => new TemplateDisplayViewModel
            {
                Template = template,
                QuestionsCount = template.Questions.Where(q => q.IsVisible).ToList().Count,
                TopicName = topics.FirstOrDefault(t => t.Id == template.TopicId).Name,
                AuthorName = users.FirstOrDefault(u => u.Id == template.CreatedByUserId).Username
            }).ToList();

            return View(viewModel);
        }

        // ����� ��������
        [HttpGet]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<Template>());
            }

            var searchResults = _context.Templates
                .Where(t => t.Name.Contains(query) || t.Description.Contains(query))
                .ToList();

            return View(searchResults);
        }
    }
}
