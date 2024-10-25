using WebForms.Interfaces;
using WebForms.Models;
using WebForms.Services;
using WebForms.ViewModels;

public class TemplateService
{
    public readonly ITemplateRepository _templateRepository;

    public readonly QuestionService _questionService;

    private readonly ImageService _imageService;

    private readonly TopicService _topicService;

    public readonly TagService _tagService;

    public readonly AccountService _accountService;

    public TemplateService(ITemplateRepository templateRepository, QuestionService questionService, ImageService imageService, TopicService topicService, TagService tagService, AccountService accountService)
    {
        _templateRepository = templateRepository;
        _questionService = questionService;
        _imageService = imageService;
        _tagService = tagService;
        _accountService = accountService;
        _topicService = topicService;
    }

    public async Task<List<Template>> GetAllTemplatesAsync()
    {
        return await _templateRepository.GetAllAsync();
    }

    public async Task<List<Template>> GetUserTemplatesAsync(int userId)
    {
        return await _templateRepository.GetTemplatesByUserAsync(userId);
    }

    public async Task CreateTemplateAsync(TemplateCreateViewModel model, int userId)
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

        await _tagService.AddTagsToTemplateAsync(model.Tags, template);

        await _imageService.AddImageToTemplateAsync(model.ImageFile, template);

        await _templateRepository.AddAsync(template);
    }

    public async Task<Template> GetTemplateByIdAsync(int id)
    {
        return await _templateRepository.GetByIdAsync(id);
    }

    public async Task UpdateQuestionsInTemplateAsync(List<Question> questions, Template template)
    {
        await _questionService.SolveQuestionsAsync(questions, template);

        await _questionService.DeleteQuestionsAsync(questions, template.Questions);
    }

    public async Task UpdateTemplateAsync(List<string> tagNames, List<Question> questions, Template template)
    {
        await _tagService.AddTagsToTemplateAsync(tagNames, template);

        await UpdateQuestionsInTemplateAsync(questions, template);

        template.UpdatedAt = DateTime.Now;

        await _templateRepository.UpdateAsync(template);
    }

    public async Task DeleteTemplateAsync(int id)
    {
        await _templateRepository.DeleteAsync(id);
    }

    public async Task<string> GetTemplateAuthorNameAsync(int userId)
    {
        return (await _accountService.GetUserByIdAsync(userId)).Username;
    }

    public async Task<List<Template>> SearchTemplatesByQueryAsync(string query)
    {
        var tagNames = await _tagService.GetAllTagNamesAsync();

        var questionTexts = await _questionService.GetAllQuestionTextsAsync();

        return await _templateRepository.FindAsync(t => t.Name.Contains(query) || t.Description.Contains(query) 
        || tagNames.Contains(query) || questionTexts.Contains(query));
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