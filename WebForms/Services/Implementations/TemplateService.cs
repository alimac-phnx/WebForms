using WebForms.Models;
using WebForms.Repositories.Interfaces;
using WebForms.Services.Interfaces;
using WebForms.ViewModels;
using FuzzySharp;

namespace WebForms.Services.Implementations
{
    public class TemplateService : ITemplateService
    {
        public readonly ITemplateRepository _templateRepository;

        public readonly IQuestionService _questionService;

        private readonly IImageService _imageService;

        private readonly ITopicService _topicService;

        public readonly ITagService _tagService;

        public readonly IAccountService _accountService;

        public TemplateService(ITemplateRepository templateRepository, IQuestionService questionService, IImageService imageService, ITopicService topicService, ITagService tagService, IAccountService accountService)
        {
            _templateRepository = templateRepository;
            _questionService = questionService;
            _imageService = imageService;
            _tagService = tagService;
            _accountService = accountService;
            _topicService = topicService;
        }

        public async Task<List<Template>> GetAllTemplatesAsync(CancellationToken cancellationToken = default)
        {
            return await _templateRepository.GetAllAsync(cancellationToken);
        }

        public async Task<List<Template>> GetUserTemplatesAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _templateRepository.GetTemplatesByUserAsync(userId, cancellationToken);
        }

        public async Task CreateTemplateAsync(TemplateCreateViewModel model, int userId, CancellationToken cancellationToken = default)
        {
            Template template = new Template()
            {
                Name = model.Name,
                Description = model.Description,
                TopicId = model.TopicId,
                Tags = new List<Tag>(),
                CreatedByUserId = userId,
                CreatedAt = DateTime.Now,
                Questions = model.Questions
            };

            await _tagService.AddTagsToTemplateAsync(model.Tags, template, cancellationToken);

            await _imageService.AddImageToTemplateAsync(model.ImageFile, template, cancellationToken);

            await _templateRepository.AddAsync(template, cancellationToken);
        }

        public async Task<Template> GetTemplateByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _templateRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task UpdateQuestionsInTemplateAsync(List<Question> questions, Template template, CancellationToken cancellationToken = default)
        {
            await _questionService.SolveQuestionsAsync(questions, template, cancellationToken);

            await _questionService.DeleteQuestionsAsync(questions, template.Questions, cancellationToken);
        }

        public async Task UpdateTemplateAsync(List<string> tagNames, List<Question> questions, Template template, CancellationToken cancellationToken = default)
        {
            await _tagService.AddTagsToTemplateAsync(tagNames, template, cancellationToken);

            await UpdateQuestionsInTemplateAsync(questions, template, cancellationToken);

            template.UpdatedAt = DateTime.Now;

            await _templateRepository.UpdateAsync(template, cancellationToken);
        }

        public async Task DeleteTemplateAsync(int id, CancellationToken cancellationToken = default)
        {
            await _templateRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<string> GetTemplateAuthorNameAsync(int userId, CancellationToken cancellationToken = default)
        {
            return (await _accountService.GetUserByIdAsync(userId, cancellationToken)).Username;
        }

        public async Task<List<Template>> SearchTemplatesByQueryAsync(string query, CancellationToken cancellationToken = default)
        {
            var allTemplates = await _templateRepository.GetAllAsync(cancellationToken);

            var allTopics = await _topicService.GetAllTopicsAsync(cancellationToken);

            var allUsers = await _accountService.GetAllUsersAsync(cancellationToken);

            return allTemplates.Where(t =>
                Fuzz.PartialRatio(query, t.Name.ToLower()) > 70 ||
                Fuzz.PartialRatio(query, t.Description.ToLower()) > 70 ||
                t.Tags.Any(tag => Fuzz.PartialRatio(query, tag.Name.ToLower()) > 70) ||
                t.Questions.Any(question => Fuzz.PartialRatio(query, question.Text.ToLower()) > 80) ||
                allTopics.Any(topic => Fuzz.PartialRatio(query, topic.Name.ToLower()) > 70) ||
                allUsers.Any(user => Fuzz.PartialRatio(query, user.Username.ToLower()) > 70)
            ).ToList();
        }

        public List<TemplateDisplayViewModel> PrepareTemplatesToDisplay(List<Template> templates)
        {
            var userIds = templates.Select(t => t.CreatedByUserId).Distinct().ToList();

            var viewModels = templates.Select(template => new TemplateDisplayViewModel
            {
                Template = template,
                QuestionsCount = template.Questions.Where(q => q.IsVisible).ToList().Count,
                TopicName = _topicService.GetTopicNameAsync(template.TopicId).Result,
                AuthorName = GetTemplateAuthorNameAsync(template.CreatedByUserId).Result
            }).ToList();

            return viewModels;
        }
    }
}