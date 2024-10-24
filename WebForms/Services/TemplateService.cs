using WebForms.Interfaces;
using WebForms.Models;
using Microsoft.IdentityModel.Tokens;
using WebForms.Services;
using WebForms.ViewModels;

public class TemplateService
{
    public readonly ITemplateRepository _templateRepository;

    public readonly ITopicRepository _topicRepository;

    public readonly ITagRepository _tagRepository;

    public readonly IQuestionRepository _questionRepository;

    public readonly QuestionService _questionService;

    private readonly ImageToCloudService _imageService;

    public TemplateService(ITemplateRepository templateRepository, ITopicRepository topicRepository, ITagRepository tagRepository, IQuestionRepository questionRepository, QuestionService questionService, ImageToCloudService imageService)
    {
        _templateRepository = templateRepository;
        _topicRepository = topicRepository;
        _tagRepository = tagRepository;
        _questionRepository = questionRepository;
        _questionService = questionService;
        _imageService = imageService;
    }

    public async Task<List<Template>> GetUserTemplates(int userId)
    {
        return await _templateRepository.GetTemplatesByUserAsync(userId);
    }

    public async Task<List<Tag>> GetAllTags()
    {
        return await _tagRepository.GetAllAsync();
    }

    public async Task<List<Topic>> GetAllTopics()
    {
        return await _topicRepository.GetAllAsync();
    }

    public async Task AddTagsToTemplate(List<string> tagNames, Template template)
    {
        var existingTags = await GetExistingTags(tagNames);

        foreach (var existingTag in existingTags)
        {
            template.Tags.Add(existingTag);
        }

        var newTagNames = GetNewTagNames(tagNames, existingTags);

        template.Tags.AddRange(await CreateNewTags(newTagNames));
    }

    public async Task<List<Tag>> GetExistingTags(List<string> tagNames)
    {
        return await _tagRepository.FindAsync(tag => tagNames.Contains(tag.Name));
    }

    public List<string> GetNewTagNames(List<string> tagNames, List<Tag> existingTags)
    {
        return tagNames.Where(tagName => !tagName.IsNullOrEmpty() && !existingTags.Any(etag => etag.Name == tagName)).ToList();
    }

    public async Task<List<Tag>> CreateNewTags(List<string> newTagNames)
    {
        var newTags = new List<Tag>();

        foreach (var tagName in newTagNames)
        {
            newTags.Add(new Tag { Name = tagName });
        }

        await _tagRepository.AddRangeAsync(newTags);

        return newTags;
    }

    public async Task AddImageToTemplate(IFormFile imageFile, Template template)
    {
        if (imageFile != null)
        {
            template.ImageUrl = await _imageService.UploadImageToCloud(imageFile);
        }
    }

    public async Task CreateTemplate(TemplateCreateViewModel model, int userId)
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

        await AddTagsToTemplate(model.Tags, template);

        await AddImageToTemplate(model.ImageFile, template);

        await _templateRepository.AddAsync(template);
    }

    public async Task<Template> GetTemplate(int id)
    {
        return await _templateRepository.GetByIdAsync(id);
    }

    public async Task UpdateQuestionsInTemplate(List<Question> questions, Template template)
    {
        await _questionService.SolveQuestions(questions, template);

        await _questionService.DeleteQuestions(questions, template.Questions);
    }

    public async Task UpdateTemplate(List<string> tagNames, List<Question> questions, Template template)
    {
        await AddTagsToTemplate(tagNames, template);

        await UpdateQuestionsInTemplate(questions, template);

        template.UpdatedAt = DateTime.Now;

        await _templateRepository.UpdateAsync(template);
    }
}